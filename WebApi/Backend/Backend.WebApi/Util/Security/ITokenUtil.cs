using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Backend.WebApi.Util.Security
{
    public interface ITokenUtil
    {
        ClaimsPrincipal GetPrincipalFromExpiredToken(string token);
        string GenerateRefreshToken(int size = 32);
        JwtSecurityToken CreateToken(IEnumerable<Claim> claims);
    }
}
