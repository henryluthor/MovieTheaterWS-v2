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

    }
}
