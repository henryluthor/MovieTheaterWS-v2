using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.EntityFrameworkCore;
using MovieTheaterWS_v2.Classes;
using MovieTheaterWS_v2.Models;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace MovieTheaterWS_v2.Services
{
    public class SystemUserService
    {
        private readonly MovietheaterContext _context;
        private readonly IConfiguration _configuration;
        private readonly UserManager<User> _userManager;
        

        public SystemUserService(MovietheaterContext context, IConfiguration configuration, UserManager<User> userManager)
        {
            _context = context;
            _configuration = configuration;
            _userManager = userManager;
        }

        public async Task<IdentityResult> SoftDeleteAndAnonymizeAsync(string id)
        {
            if (string.IsNullOrEmpty(id)) throw new ArgumentNullException("id");

            var user = await _userManager.FindByIdAsync(id);

            if (user == null)
            {
                return IdentityResult.Failed( new IdentityError { Description = "User not found."});
            }

            // Generate unique identifiers for fields
            string uniqueId = Guid.NewGuid().ToString("N").Substring(0, 8);
            string anonymousEmail = $"deleted_{uniqueId}@domain.com";
            string anonymousUserName = $"deleted_user_{uniqueId}";

            // Anonymize sensible data
            user.IsDeleted = true;

            // Identity uses Email and UserName for login and uniqueness validations
            user.Email = anonymousEmail;
            user.NormalizedEmail = anonymousEmail.ToUpper();
            user.UserName = anonymousUserName;
            user.NormalizedUserName = anonymousUserName.ToUpper();

            user.FirstName = "Anonymized";
            user.LastName = "User";

            // Security: Invalidate sessions and block access
            await _userManager.RemovePasswordAsync(user);
            user.PasswordHash = null; // Will not be able to log in again
            user.LockoutEnd = DateTimeOffset.MaxValue; // Permanent block
            user.SecurityStamp = Guid.NewGuid().ToString(); // Closes active sessions

            return await _userManager.UpdateAsync(user);             

        }



        // Deprecated, kept for code reference
        // Validates the login request and if valid returns a token, otherwise returns an empty string
        //public async Task<string> ValidateLoginRequest(Classes.LoginRequest loginRequestClass)
        //{
        //    LoginTokenGenerator loginTokenGenerator = new LoginTokenGenerator(_configuration, _userManager);

        //    string token = string.Empty;

        //    // Check if email in the request exist in DB
        //    User systemUser = await _context.Users.FirstOrDefaultAsync(s => s.Email == loginRequestClass.Email);

        //    if (systemUser != null)
        //    {
        //        // Hash the received password and check if it matches the hash in DB
        //        SHA512 hashSvc = SHA512.Create();
        //        byte[] hash = hashSvc.ComputeHash(Encoding.UTF8.GetBytes(loginRequestClass.Password));
        //        string hashString = BitConverter.ToString(hash).Replace("-", "");

        //        if (systemUser.PasswordHash == hashString)
        //        {
        //            // Password is correct
        //            string username;
        //            if (systemUser.FirstName != null)
        //            {
        //                username = systemUser.FirstName;
        //            }
        //            else
        //            {
        //                username = "Generic name";
        //            }

        //            token = await loginTokenGenerator.GenerateToken(systemUser);
        //        }
        //    }

        //    return token;
        //}
    }
}
