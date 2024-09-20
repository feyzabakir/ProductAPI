using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using ProductAPI.Core.DTOs.Authentication;
using ProductAPI.Service.Authorization.Abstraction;
using ProductAPI.Service.Helpers;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ProductAPI.Service.Authorization.Concrete
{
    public class JwtAuthenticationManager : IJwtAuthenticationManager
    {
        private readonly AppSettings _appSettings;
        private readonly UserManager<IdentityUser> _userManager;  

        public JwtAuthenticationManager(IOptions<AppSettings> appSettings, UserManager<IdentityUser> userManager)
        {
            _appSettings = appSettings.Value;
            _userManager = userManager;  
        }

        public AuthResponseDto Authenticate(string userName, string password)
        {
            AuthResponseDto authResponse = new AuthResponseDto();

            try
            {
                var user = _userManager.FindByNameAsync(userName).Result;  // Kullanıcıyı bul
                if (user == null)
                {
                    return authResponse;  // Kullanıcı yoksa boş dön
                }

                var roles = _userManager.GetRolesAsync(user).Result;  // Kullanıcının rollerini al

                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(_appSettings.Secret);

                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, userName),
                    new Claim(ClaimTypes.NameIdentifier, user.Id)  // Kullanıcı ID'si
                };

                // Kullanıcının rollerini JWT token'a claim olarak ekle
                claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Expires = DateTime.UtcNow.AddHours(1),
                    Subject = new ClaimsIdentity(claims),  
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                };

                var token = tokenHandler.CreateToken(tokenDescriptor);
                authResponse.Token = tokenHandler.WriteToken(token);

                return authResponse;
            }
            catch (Exception)
            {
                return authResponse;
            }
        }

        public string? ValidateJwtToken(string token)
        {
            if (token == null)
                return null;

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            try
            {
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

                var jwtToken = (JwtSecurityToken)validatedToken;
                var userId = jwtToken.Claims.First(x => x.Type == ClaimTypes.NameIdentifier).Value;

                return userId;
            }
            catch
            {
                return null;
            }
        }
    }
}


