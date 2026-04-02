using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.EntityFrameworkCore;
using MovieTheaterWS_v2.Classes;
using MovieTheaterWS_v2.Models;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace MovieTheaterWS_v2.Bll
{
    public class SystemUserBll
    {
        private readonly MovietheaterContext _context;
        private readonly IConfiguration _configuration;
        private readonly UserManager<User> _userManager;
        

        public SystemUserBll(MovietheaterContext context, IConfiguration configuration, UserManager<User> userManager)
        {
            _context = context;
            _configuration = configuration;
            _userManager = userManager;
        }

        // Validates the login request and if valid returns a token, otherwise returns an empty string
        public async Task<string> ValidateLoginRequest(Classes.LoginRequest loginRequestClass)
        {
            LoginTokenGenerator loginTokenGenerator = new LoginTokenGenerator(_configuration, _userManager);

            string token = string.Empty;

            // Check if email in the request exist in DB
            User systemUser = await _context.Users.FirstOrDefaultAsync(s => s.Email == loginRequestClass.Email);

            if (systemUser != null)
            {
                // Hash the received password and check if it matches the hash in DB
                SHA512 hashSvc = SHA512.Create();
                byte[] hash = hashSvc.ComputeHash(Encoding.UTF8.GetBytes(loginRequestClass.Password));
                string hashString = BitConverter.ToString(hash).Replace("-", "");

                if (systemUser.PasswordHash == hashString)
                {
                    // Password is correct
                    string username;
                    if (systemUser.FirstName != null)
                    {
                        username = systemUser.FirstName;
                    }
                    else
                    {
                        username = "Generic name";
                    }

                    token = await loginTokenGenerator.GenerateToken(systemUser);
                }
            }

            return token;
        }
    }
}
