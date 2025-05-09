using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using BookingApp.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

namespace BookingApp.Api.Services
{
    public static class TokenManager
    {
        public static string GenerateJwtToken(User user, UserManager<User> userManager, IConfiguration configuration)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(configuration["Jwt:Key"]);

            // Получаем роли пользователя
            var roles = userManager.GetRolesAsync(user).Result;

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.Email, user.Email ?? string.Empty)
            };

            // Добавляем роли в claims
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var expireDays = configuration.GetValue<int>("Jwt:ExpireDays");
            var notBefore = DateTime.UtcNow.AddMinutes(-1);
            var expires = notBefore.AddDays(expireDays);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                NotBefore = notBefore,
                Expires = expires,
                Issuer = configuration["Jwt:Issuer"],
                Audience = configuration["Jwt:Audience"],
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}