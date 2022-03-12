using AutoMapper;
using Backend.Contract.Security;
using Backend.DAL.Models;
using Backend.DAL.Security;
using Backend.WebApi.Mappings;
using Backend.WebApi.Util.Security;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System;
using System.IO;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Backend.WebApi
{
  public class Startup
  {
    public IConfiguration Configuration { get; }

    public Startup(IConfiguration configuration)
    {
      Configuration = configuration;
    }

    public void ConfigureServices(IServiceCollection services)
    {
      services.AddDbContext<FrontedContext>(options => options.UseSqlServer(Configuration.GetConnectionString("FrontEd")));

      services.AddControllers()
              .AddJsonOptions(configure => configure.JsonSerializerOptions.PropertyNamingPolicy = null);
      

      services.AddSwaggerGen(c =>
      {
        c.SwaggerDoc(Constants.ApiVersion, new OpenApiInfo
        {
          Title = "FrontEd Workshop 2022",
          Version = Constants.ApiVersion
        });
        var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
        var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
        c.IncludeXmlComments(xmlPath);

        //e.g. include comment from other projects (not needed, bust just for demonstration)
        xmlFile = $"{typeof(Contract.DTOs.School).Assembly.GetName().Name}.xml";
        xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
        c.IncludeXmlComments(xmlPath);

        var jwtSecurityScheme = new OpenApiSecurityScheme
        {
          Description = "JWT Authorization header using the Bearer scheme, i.e. \"Authorization: Bearer {token}\". Paste token only to the text field ",
          Name = "Authorization",
          In = ParameterLocation.Header,
          Type = SecuritySchemeType.Http,
          Scheme = "bearer",
          Reference = new OpenApiReference
          {
            Id = JwtBearerDefaults.AuthenticationScheme,
            Type = ReferenceType.SecurityScheme
          }
        };

        c.AddSecurityDefinition(JwtBearerDefaults.AuthenticationScheme, jwtSecurityScheme);

        c.AddSecurityRequirement(new OpenApiSecurityRequirement
        {
          { jwtSecurityScheme, Array.Empty<string>() }
        });
      });

      SetupAuth(services);

      Action<IServiceProvider, IMapperConfigurationExpression> mapperConfigAction = (serviceProvider, cfg) =>
      {
        cfg.ConstructServicesUsing(serviceProvider.GetService);        
      };
      services.AddAutoMapper(mapperConfigAction, typeof(EFMappingProfile)); //assemblies containing mapping profiles            
    }

    //https://docs.microsoft.com/en-us/aspnet/core/fundamentals/middleware/?view=aspnetcore-5.0
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
      #region Used for nginx + Kestrel
      app.UseForwardedHeaders(new ForwardedHeadersOptions
      {
        ForwardedHeaders = ForwardedHeaders.XForwardedFor |
                           ForwardedHeaders.XForwardedProto
      });
      string pathBase = Configuration["PathBase"];
      if (!string.IsNullOrWhiteSpace(pathBase))
      {
        app.UsePathBase(pathBase);
      }
      #endregion

      if (env.IsDevelopment())
      {
        app.UseDeveloperExceptionPage();
      }

      app.UseSwagger();
      app.UseSwaggerUI(c =>
      {              
        c.RoutePrefix = "docs";
        c.DocumentTitle = "FrontEd Workshop Demo WebAPI";
        c.SwaggerEndpoint($"../swagger/{Constants.ApiVersion}/swagger.json", c.DocumentTitle);
      });
   
      
      app.UseRouting();

      app.UseCors(builder =>
      {
        builder.AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader()
               .WithExposedHeaders("Token-Expired");
      });

      app.UseAuthentication();
      app.UseAuthorization();

      app.UseEndpoints(endpoints =>
      {
        endpoints.MapControllers();
      });
    }

    private void SetupAuth(IServiceCollection services)
    {
      services.AddTransient<IPasswordHasher<string>, PasswordHasher<string>>();
      services.AddTransient<IUserManagementService, UserManagementService>();
      services.AddTransient<ITokenUtil, TokenUtil>();

      var tokenSection = Configuration.GetSection("TokenConfiguration");
      services.Configure<TokenConfig>(tokenSection);
      var token = tokenSection.Get<TokenConfig>();
      var secret = Encoding.Default.GetBytes(token.Secret);

      services.AddAuthentication(opt =>
              {
                opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
              })
              .AddJwtBearer(opt =>
              {
                opt.SaveToken = true;
                opt.TokenValidationParameters = new TokenValidationParameters
                {
                  ValidateIssuer = true,
                  ValidateAudience = true,
                  ValidateLifetime = true,
                  ValidateIssuerSigningKey = true,

                  IssuerSigningKey = new SymmetricSecurityKey(secret),
                  ValidIssuer = token.Issuer,
                  ValidAudience = token.Audience,
                };
                opt.Events = new JwtBearerEvents
                {
                  OnAuthenticationFailed = context =>
                  {
                    if (context.Exception.GetType() == typeof(SecurityTokenExpiredException))
                    {
                      context.Response.Headers.Add("Token-Expired", "true");
                    }
                    return Task.CompletedTask;
                  }
                };
              });

      services.AddAuthorization(options =>
      {
        foreach (var policy in Policies.All)
        {
          options.AddPolicy(policy.Key, policy.Value);
        }
      });
    }

  }
}
