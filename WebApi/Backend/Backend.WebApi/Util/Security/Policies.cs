using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Backend.WebApi.Util.Security
{
    public class Policies
    {
        public static IEnumerable<KeyValuePair<string, Action<AuthorizationPolicyBuilder>>> All
        {
            get
            {                
                //yield return new KeyValuePair<string, Action<AuthorizationPolicyBuilder>>(nameof(Admin), Admin);
                yield return new KeyValuePair<string, Action<AuthorizationPolicyBuilder>>(nameof(ReadAccess), ReadAccess);
            }
        }

    public static Action<AuthorizationPolicyBuilder> ReadAccess
    {
      get
      {
        Predicate<Claim> match = c => c.Type == ClaimTypes.Role; // && (c.Value == nameof(UserDto.Admin) || c.Value == nameof(UserDto.Update) || c.Value == nameof(UserDto.Create));
        return policy => policy.RequireAssertion(context => context.User.HasClaim(match));
      }
    }

    //public static Action<AuthorizationPolicyBuilder> Admin
    //{
    //    get
    //    {
    //        return policy => policy.RequireClaim(ClaimTypes.Role, nameof(UserDto.Admin));
    //    }
    //}
  }
}
