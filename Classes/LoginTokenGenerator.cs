using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MovieTheaterWS_v2.Classes
{
    public class LoginTokenGenerator
    {
        private int tokenExpirationMinutes = 3;
        private readonly IConfiguration _configuration;

        public LoginTokenGenerator(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public string GenerateToken(string userId, string username)
        {
            string mySecretKey = _configuration.GetValue<string>("JwtBearerData:SecretKey");

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, userId),
                new Claim(ClaimTypes.Name, username)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(mySecretKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration.GetValue<string>("JwtBearerData:Issuer"),
                audience: _configuration.GetValue<string>("JwtBearerData:Audience"),
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(tokenExpirationMinutes),
                signingCredentials: creds
                );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
