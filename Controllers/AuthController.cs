using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using MovieTheaterWS_v2.Classes;
using MovieTheaterWS_v2.Models;
using NuGet.Protocol;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Cryptography;
using System.Text;

namespace MovieTheaterWS_v2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private int cookieExpirationHours = 24;
        private readonly MovietheaterContext _context;
        private readonly IConfiguration _configuration;

        public AuthController(MovietheaterContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        // Original line
        // public IActionResult Login([FromBody] LoginRequest loginRequest)
        // Generate token and create cookie to store token
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest loginRequest)
        {            

            // Need LoginResponse object to generate token
            LoginResponse loginResponse = new LoginResponse(_configuration);

            var token = String.Empty;

            // Authenticate user
            // Check if email exist in DB
            var systemUser = new Systemuser();

            try
            {
                systemUser = await _context.Systemusers.FirstOrDefaultAsync(s => s.Email == loginRequest.Email);
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
                        if (systemUser.FirstName != null)
                        {
                            username = systemUser.FirstName;
                        }
                        else
                        {
                            username = "Generic name";
                        }
                        token = loginResponse.GenerateToken(systemUser.Id.ToString(), username);

                        var cookieOptions = new CookieOptions
                        {
                            //HttpOnly = false,
                            Secure = true,
                            Domain = "localhost",
                            Path = "/",
                            Expires = DateTime.UtcNow.AddHours(cookieExpirationHours),
                            //IsEssential = true,
                            SameSite = SameSiteMode.None,
                        };
                        Response.Cookies.Append("token", token, cookieOptions);

                        // This is how you set text in the body of the HttpResponse
                        //await Response.WriteAsync("Hello, this is the HttpResponse body");
                        //return Ok();

                        // This is how you set an object in the body of the HttpResponse
                        //var persona = new
                        //{
                        //    Name = "Juan",
                        //    Age = 30
                        //};
                        //await Response.WriteAsJsonAsync(persona);
                        //return Ok();

                        // This is how you set an object as an action result
                        //var persona = new
                        //{
                        //    Name = "Juan",
                        //    Age = 30
                        //};
                        //return Ok(persona);

                        return Ok(new { authenticated = true });
                    }
                    else
                    {
                        // Incorrect user or password
                        return Unauthorized();
                    }
                }
                else
                {
                    return NotFound();
                }
            }
            catch
            {
                return BadRequest();
            }            
        }

        private string GenerateToken()
        {
            // Generate token
            return "token generated";
        }

        // Verify token
        [HttpGet("authenticated")]
        public IActionResult Authenticated()
        {
            //var token = Request.Cookies["token"];
            var token = HttpContext.Request.Cookies["token"];

            if(token == null)
            {
                return Unauthorized();
            }

            var tokenHandler = new JwtSecurityTokenHandler();
            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = "MovieTheaterWS-v2", //check this
                ValidAudience = "movie-theater-web-app", //check this
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("H7YVZWs1TnxVF8tCOCLF2/RJRy0FK3Hk")) //check this
            };

            try
            {
                var claimsPrincipal = tokenHandler.ValidateToken(token, validationParameters, out var securityToken);

                if(securityToken is JwtSecurityToken jwtSecurityToken)
                {
                    var expirationDate = jwtSecurityToken.ValidTo;

                    if(expirationDate < DateTime.UtcNow)
                    {
                        // Token has expired
                        return Unauthorized();
                    }

                    return Ok(new { authenticated = true });
                }
                else
                {
                    // Token is not JWT
                    return Unauthorized();
                }
            }
            catch(SecurityTokenException ex)
            {
                // Token is invalid
                return Unauthorized();
            }

            //if(token != null)
            //{
            //    var x = token;
            //    // Verify token
            //    return Ok(new { authenticated = true });
            //}
            //else
            //{
            //    return Ok(new { authenticated = false });
            //}
        }
    }
}
