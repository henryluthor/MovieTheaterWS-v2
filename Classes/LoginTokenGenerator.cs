using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using MovieTheaterWS_v2.Models;

namespace MovieTheaterWS_v2.Classes
{
    public class LoginTokenGenerator
    {
        private int tokenExpirationMinutes = 3;

        private readonly UserManager<User> _userManager;
        private readonly IConfiguration _configuration;
        

        public LoginTokenGenerator(IConfiguration configuration, UserManager<User> userManager)
        {
            _userManager = userManager;
            _configuration = configuration;
        }


        public async Task<string> GenerateToken(User user)
        {
            var userRoles = await _userManager.GetRolesAsync(user);            

            var authClaims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            };

            // Add roles to the Claims list so token knows it is Admin
            foreach (var role in userRoles)
            {
                authClaims.Add(new Claim(ClaimTypes.Role, role));
            }

            string mySecretKey = _configuration.GetValue<string>("JwtBearerData:SecretKey");
            // Another form of doing the same as line above
            // string mySecretKey = _configuration["JwtBearerData:SecretKey"];
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(mySecretKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration.GetValue<string>("JwtBearerData:Issuer"),
                audience: _configuration.GetValue<string>("JwtBearerData:Audience"),
                claims: authClaims,
                expires: DateTime.UtcNow.AddMinutes(tokenExpirationMinutes),
                signingCredentials: creds
                );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
