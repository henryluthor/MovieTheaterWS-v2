using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieTheaterWS_v2.Classes;
using MovieTheaterWS_v2.Models;
//using System.Security.Cryptography;
//using System.Text;

namespace MovieTheaterWS_v2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SystemUserController : ControllerBase
    {
        private readonly MovietheaterContext _context;
        private readonly UserManager<User> _userManager;

        public SystemUserController(MovietheaterContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: api/<SystemUserController>
        [HttpGet]
        public async Task<List<User>> Get()
        {
            //return await _context.Systemusers.ToListAsync();
            return await _context.Users.ToListAsync();
        }

        // Deprecated since using AspNetCore.Identity
        // POST: api/<SystemUserController>
        //[HttpPost]
        ////[Authorize(Policy = "AdminOnly")]
        //[Authorize(Roles = "Admin")]
        //public async Task<GenericResponse<User>> PostOld(SystemUserToPost systemUserToPost)
        //{
        //    var genRensponse = new GenericResponse<User>();

        //    var allSystemUsers = from s in _context.Users select s;
        //    allSystemUsers = allSystemUsers.Where(s => s.Email == systemUserToPost.Email);

        //    // Check if email is already registered
        //    if (allSystemUsers.Any())
        //    {
        //        genRensponse.Message = "That email is registered already.";
        //    }
        //    else
        //    {
        //        try
        //        {
        //            // Hash user password
        //            SHA512 hashSvc = SHA512.Create();
        //            byte[] hash = hashSvc.ComputeHash(Encoding.UTF8.GetBytes(systemUserToPost.Password));

        //            User systemUser = new User();
        //            systemUser.FirstName = systemUserToPost.FirstName;
        //            systemUser.LastName = systemUserToPost.LastName;
        //            systemUser.Email = systemUserToPost.Email;
        //            systemUser.PasswordHash = BitConverter.ToString(hash).Replace("-", "");
        //            //systemUser.IdRole = userToPost.IdRole;

        //            _context.Users.Add(systemUser);
        //            await _context.SaveChangesAsync();

        //            genRensponse.Message = "User registered successfully.";
        //            genRensponse.Data = systemUser;
        //        }
        //        catch (Exception ex)
        //        {
        //            genRensponse.Message = "There was an error while trying to register the user. " + ex.Message;
        //        }
        //    }

        //    return genRensponse;
        //}


        [AllowAnonymous]
        [HttpPost("register-customer")]
        public async Task<IActionResult> RegisterCustomer([FromBody] CustomerRegistrationDTO userToPost)
        {
            var user = new User
            {
                UserName = userToPost.Email,
                Email = userToPost.Email
            };

            // It is not necessary to manually hash the password, CreateAsync does it
            var result = await _userManager.CreateAsync(user, userToPost.Password);

            if (result.Succeeded)
            {
                // User created successfully
                // Forced role assignation from server, ignoring any attempt from client
                await _userManager.AddToRoleAsync(user, "Customer");
                return Ok();
            }
            //return BadRequest(result.Errors.Select(e => e.Description));
            return BadRequest(result.Errors);
        }



        [Authorize(Roles = "Admin")]
        [HttpPost("create-admin-user")]
        public async Task<IActionResult> CreateAdminUser([FromBody] AdminRegistrationDTO userToPost)
        {
            var user = new User
            {
                UserName = userToPost.Email,
                Email = userToPost.Email
            };

            // It is not necessary to manually hash the password, CreateAsync does it
            var result = await _userManager.CreateAsync(user, userToPost.Password);
            if (result.Succeeded)
            {
                // User created successfully
                // Here you trust the model because only an Admin reached this point
                await _userManager.AddToRoleAsync(user, userToPost.RoleName);
                return Ok();
            }
            //return BadRequest(result.Errors.Select(e => e.Description));
            return BadRequest(result.Errors);
        }

    }
}
