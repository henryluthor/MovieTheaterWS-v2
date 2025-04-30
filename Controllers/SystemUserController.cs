using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieTheaterWS_v2.Classes;
using MovieTheaterWS_v2.Models;
using System.Security.Cryptography;
using System.Text;

namespace MovieTheaterWS_v2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SystemUserController : ControllerBase
    {
        private readonly MovietheaterContext _context;

        public SystemUserController(MovietheaterContext context)
        {
            _context = context;
        }

        // GET: api/<SystemUserController>
        [HttpGet]
        public async Task<List<SystemUser>> Get()
        {
            return await _context.Systemusers.ToListAsync();
        }

        // POST: api/<SystemUserController>
        [HttpPost]
        public async Task<GenericResponse<SystemUser>> Post(SystemUserToPost systemUserToPost)
        {
            var genRensponse = new GenericResponse<SystemUser>();

            var allSystemUsers = from s in _context.Systemusers select s;
            allSystemUsers = allSystemUsers.Where(s => s.Email == systemUserToPost.Email);
            //List<Systemuser> systemUsersFiltered = await allSystemUsers.ToListAsync();

            if (allSystemUsers.Any())
            {
                genRensponse.Message = "That email is registered already.";
            }
            else
            {
                // Hash user password
                SHA512 hashSvc = SHA512.Create();
                byte[] hash = hashSvc.ComputeHash(Encoding.UTF8.GetBytes(systemUserToPost.Password));

                SystemUser systemUser = new SystemUser();
                systemUser.Email = systemUserToPost.Email;
                systemUser.PasswordHash = BitConverter.ToString(hash).Replace("-", "");
                systemUser.IdRole = systemUserToPost.IdRole;

                _context.Systemusers.Add(systemUser);
                await _context.SaveChangesAsync();

                genRensponse.Message = "User registered successfully.";
                genRensponse.Data = systemUser;
            }

            return genRensponse;
        }

        // POST: api/<SystemUserController>
        //[HttpPost]
        //public async Task<GenericResponse<string>> Loginv2(string email, string password)
        //{
        //    GenericResponse<string> genericResponse = new GenericResponse<string>();

        //    // Check if email exist in DB
        //    var systemUser = new SystemUser();
        //    systemUser = await _context.Systemusers.FirstOrDefaultAsync(s => s.Email == email);

        //    if (systemUser != null)
        //    {
        //        // Hash received password and check if it matches the hash in DB
        //        SHA512 hashSvc = SHA512.Create();
        //        byte[] hash = hashSvc.ComputeHash(Encoding.UTF8.GetBytes(password));
        //        string hashString2 = Encoding.UTF8.GetString(hash);
        //        string hashString = BitConverter.ToString(hash).Replace("-", "");

        //        if (systemUser.PasswordHash == hashString)
        //        {
        //            // Access granted
        //            genericResponse.Message = "Login successfull. hashString: " + hashString + ", hashString2: " + hashString2;
        //        }
        //        else
        //        {
        //            // Incorrect user or password
        //            genericResponse.Message = "Incorrect user or password.";
        //        }
        //    }
        //    else
        //    {
        //        genericResponse.Message = "User not registered.";
        //    }

        //    return genericResponse;
        //}

        //public IActionResult Index()
        //{
        //    return View();
        //}
    }
}
