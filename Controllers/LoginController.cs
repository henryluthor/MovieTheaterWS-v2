using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieTheaterWS_v2.Classes;
using MovieTheaterWS_v2.Models;
using System.Security.Cryptography;
using System.Text;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MovieTheaterWS_v2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly MovietheaterContext _context;

        public LoginController(MovietheaterContext context)
        {
            _context = context;
        }

        // GET: api/<LoginController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<LoginController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<LoginController>
        [HttpPost]
        public async Task<GenericResponse<SystemUser>> Post([FromBody] LoginRequest loginRequest)
        {
            GenericResponse<SystemUser> genericResponse = new GenericResponse<SystemUser>();
            
            // Check if email exist in DB
            var systemUser = new SystemUser();
            systemUser = await _context.Systemusers.FirstOrDefaultAsync(s => s.Email == loginRequest.Email);

            if (systemUser != null)
            {
                // Hash received password and check if it matches the hash in DB
                SHA512 hashSvc = SHA512.Create();
                byte[] hash = hashSvc.ComputeHash(Encoding.UTF8.GetBytes(loginRequest.Password));
                string hashString = BitConverter.ToString(hash).Replace("-", "");

                if (systemUser.PasswordHash == hashString)
                {
                    // Access granted
                    genericResponse.Message = "Login successfull.";
                    genericResponse.Data = systemUser;
                }
                else
                {
                    // Incorrect user or password
                    genericResponse.Message = "Incorrect user or password.";
                }
            }
            else
            {
                // User not registered
                //genericResponse.Message = "User not registered.";
                genericResponse.Message = "Incorrect user or password.";
            }

            return genericResponse;
        }

        // PUT api/<LoginController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/<LoginController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
