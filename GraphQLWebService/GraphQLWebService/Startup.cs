using GraphQL.Server.Ui.Voyager;
using GraphQLWebService.GraphQLSetup;
using GraphQLWebService.Models;
using HotChocolate.Types.Pagination;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GraphQLWebService
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
      //Note: Cannot run in parallel, see https://chillicream.com/docs/hotchocolate/integrations/entity-framework for workaround
      services.AddDbContext<FrontedContext>(options => options.UseSqlServer(Configuration.GetConnectionString("FrontEd")));
      services.AddGraphQLServer()
              .SetPagingOptions(new PagingOptions
              {
                DefaultPageSize = 20,
                MaxPageSize = 1000,
                IncludeTotalCount = true
              })
              .AddProjections()
              .AddFiltering()
              .AddSorting()
              .AddQueryType<Queries>()
              .AddMutationType<Mutations>();
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
      if (env.IsDevelopment())
      {
        app.UseDeveloperExceptionPage();
      }

      app.UseStaticFiles();
      app.UseRouting();

      app.UseEndpoints(endpoints =>
      {
        endpoints.MapGraphQL();
      });

      app.UseGraphQLVoyager(new VoyagerOptions()
                            {
                              GraphQLEndPoint = "/graphql"
                            }, "/graphql-voyager");
    }
  }
}
