using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MovieTheaterWS_v2.Classes
{
    public class LoginResponse
    {
        private readonly IConfiguration _configuration;

        public LoginResponse(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public bool Success {  get; set; }
        public int userId { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; } = null!;
        public int? IdRole { get; set; }
        public string? SecretKey { get; set; }

        public string GenerateToken(string userId, string username)
        {
            string mySecretKey;
            mySecretKey = _configuration.GetValue<string>("JwtBearerData:SecretKey");

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
                expires: DateTime.UtcNow.AddMinutes(1),
                signingCredentials: creds
                );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
