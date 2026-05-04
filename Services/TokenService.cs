using Blog.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Blog.Services
{
    public class TokenService
    {
        public string GenerateToken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(Configuration.JwtKey);
            var agora = DateTime.UtcNow.AddHours(-3);


            //Console.WriteLine($"JWT KEY TokenService: [{Configuration.JwtKey}]");
            //Console.WriteLine($"JWT KEY TokenService LENGTH: {Configuration.JwtKey.Length}");
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new (ClaimTypes.Name, "mathmantovan"), //User.Identity.Name ESSE CLAIM PERMITE A VERIVICAÇÃO POR NOME
                    new (ClaimTypes.Role, "admin"),//User.Identity.Role ESSE CLAIM PERMITE A VERIVICAÇÃO POR TIPO DE PERFIL
                    new (ClaimTypes.Role, "user"),
                }),
                Expires = DateTime.Now.AddHours(8),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature)
            };


            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);


        }

    }
}
