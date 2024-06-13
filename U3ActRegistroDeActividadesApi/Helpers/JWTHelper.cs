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
        public static string GetToken(Departamentos depto, JWT jwt)
        {
            var claims = new List<Claim>
            {
                new("id", depto.Id.ToString()),
                new("idSuperior", depto.IdSuperior?.ToString() ?? ""),
                new(ClaimTypes.Name, depto.Nombre),
                new(ClaimTypes.Role, depto.IdSuperior>0?"Administrador":"Departamento")
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwt.Key));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: jwt.Issuer,
                audience: jwt.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddHours(2),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
