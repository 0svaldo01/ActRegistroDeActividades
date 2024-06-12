using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using U3ActRegistroDeActividadesApi.Models.Entities;
using U3ActRegistroDeActividadesApi.Models.Security;

namespace U3ActRegistroDeActividadesApi.Helpers
{
    public class JWTHelper
    {
        public static string GetToken(Departamentos depto, JWT JWT)
        {
            List<Claim> Claims = [
                new Claim("id",depto.Id.ToString()),
                new Claim("idSuperior",depto.IdSuperior.ToString()??""),
                new Claim(ClaimTypes.Name,depto.Nombre),
                new Claim(ClaimTypes.Role,"Admin")
                ];
            JwtSecurityTokenHandler handler = new();
            var Credentials = new SigningCredentials(
                new SymmetricSecurityKey
                (
                     Encoding.UTF8.GetBytes(JWT.Key)
                ),
                SecurityAlgorithms.HmacSha256
            );
            var token = handler.CreateToken(new SecurityTokenDescriptor()
            {
                Issuer = JWT.Issuer,
                Audience = JWT.Audience,
                IssuedAt = DateTime.UtcNow,
                Expires = DateTime.UtcNow.AddHours(2),
                NotBefore = DateTime.UtcNow,
                SigningCredentials = Credentials,
                Subject = new ClaimsIdentity(Claims, JwtBearerDefaults.AuthenticationScheme)
            });
            return handler.WriteToken(token);
        }
    }
}
