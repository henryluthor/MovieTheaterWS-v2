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
        private readonly IConfiguration _configuration;

        public LoginController(MovietheaterContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
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
        public async Task<GenericResponse<LoginResponse>> Post([FromBody] LoginRequest loginRequest)
        {
            GenericResponse<LoginResponse> genericResponse = new GenericResponse<LoginResponse>();
            
            // Check if email exist in DB
            var systemUser = new SystemUser();
            systemUser = await _context.Systemusers.FirstOrDefaultAsync(s => s.Email == loginRequest.Email);

            LoginResponse loginResponse = new LoginResponse(_configuration);

            if (systemUser != null)
            {
                // Hash the received password and check if it matches the hash in DB
                SHA512 hashSvc = SHA512.Create();
                byte[] hash = hashSvc.ComputeHash(Encoding.UTF8.GetBytes(loginRequest.Password));
                string hashString = BitConverter.ToString(hash).Replace("-", "");                

                if (systemUser.PasswordHash == hashString)
                {
                    // Access granted
                    string username;
                    if(systemUser.FirstName != null)
                    {
                        username = systemUser.FirstName;
                    }
                    else
                    {
                        username = "Generic name";
                    }

                    loginResponse.Success = true;
                    loginResponse.FirstName = systemUser.FirstName;
                    loginResponse.LastName = systemUser.LastName;
                    loginResponse.Email = systemUser.Email;
                    loginResponse.IdRole = systemUser.IdRole;
                    loginResponse.Token = loginResponse.GenerateToken(systemUser.Id.ToString(), username);
                    
                    genericResponse.Message = "Login successfull.";
                }
                else
                {
                    // Incorrect user or password
                    loginResponse.Success = false;
                    genericResponse.Message = "Incorrect user or password.";
                }
            }
            else
            {
                // User not registered
                loginResponse.Success = false;
                //genericResponse.Message = "User not registered.";
                genericResponse.Message = "Incorrect user or password.";
            }

            genericResponse.Data = loginResponse;

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
