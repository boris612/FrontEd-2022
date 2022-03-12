using Backend.Contract.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace Backend.WebApi.Util.Security
{
  public static class SecurityExtensions
  {
    public static int GetUserId(this ClaimsPrincipal user)
    {
      Claim claim = user.Claims.Where(c => c.Type == ClaimTypes.Sid).FirstOrDefault();
      if (claim == null || !int.TryParse(claim.Value, out int id))
      {
        throw new Exception($"Cannot find SID for user: {user.Identity.Name}");
      }
      return id;
    }

    public static bool IsAdmin(this ClaimsPrincipal user)
    {
      return false;
      //Predicate<Claim> match = c => c.Type == ClaimTypes.Role && c.Value == nameof(UserDto.Admin);
      //return user.HasClaim(match);
    }

    public static List<Claim> CreateClaims(this UserDto user)
    {
      var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Sid, user.Id.ToString()),
                    new Claim(ClaimTypes.Name, user.Username),
                    new Claim(ClaimTypes.GivenName, user.FirstName),
                    new Claim(ClaimTypes.Surname, user.LastName)
                };

      //if (user.Admin)
      //{
      //    claims.Add(new Claim(ClaimTypes.Role, nameof(user.Admin)));
      //}
      //if (user.Update)
      //{
      //    claims.Add(new Claim(ClaimTypes.Role, nameof(user.Update)));
      //}
      //if (user.Create)
      //{
      //    claims.Add(new Claim(ClaimTypes.Role, nameof(user.Create)));
      //}
      //if (user.Delete)
      //{
      //    claims.Add(new Claim(ClaimTypes.Role, nameof(user.Delete)));
      //}
      return claims;
    }
  }
}
