using Microsoft.IdentityModel.Tokens;
using ProyectoApi_Sabado.Services;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ProyectoApi_Sabado.Models
{
    public class UtilitariosModel : IUtilitariosModel
    {

        public string GenerarToken(string cedula)
        {
            List<Claim> claims = new List<Claim>();
            claims.Add(new Claim("username", cedula));

            string SecretKey = "erQuPVWaBcnFePyQEGRhDjFCzbtGBLgL";
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(SecretKey));
            var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(10),
                signingCredentials: cred);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

    }
}
