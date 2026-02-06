using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MovieTheaterWS_v2.Bll;
using MovieTheaterWS_v2.Classes;
using MovieTheaterWS_v2.Models;
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
        // public IActionResult Login([FromBody] LoginRequest loginRequestClass)
        // Generate token and create cookie to store token
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestClass loginRequestClass)
        {
            SystemUserBll systemUserBll = new SystemUserBll(_context, _configuration);
            var token = string.Empty;

            try
            {
                token = await systemUserBll.ValidateLoginRequest(loginRequestClass);

                if (token.IsNullOrEmpty())
                {
                    // Incorrect user or password
                    return Unauthorized();                    
                }
                else
                {
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
            }
            catch
            {
                return BadRequest();
            }            
        }


        // Verify token
        [HttpGet("authenticated")]
        public IActionResult Authenticated()
        {
            // var token = Request.Cookies["token"]; // this is another way of doing the following line
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

        }
    }
}
