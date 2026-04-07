using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MovieTheaterWS_v2.Classes;
using MovieTheaterWS_v2.Models;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;


namespace MovieTheaterWS_v2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private int cookieExpirationHours = 24;
        
        //private readonly MovietheaterContext _context;
        //private readonly IConfiguration _configuration;

        private readonly LoginTokenGenerator _tokenGenerator;
        private readonly UserManager<User> _userManager;

        public AuthController(LoginTokenGenerator tokenGenerator, UserManager<User> userManager)
        {
            //_context = context;
            //_configuration = configuration;

            _tokenGenerator = tokenGenerator;
            _userManager = userManager;
        }

        
        // Generate token and create cookie to store token
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest loginRequest)
        {

            var user = await _userManager.FindByEmailAsync(loginRequest.Email);

            if(user != null && await _userManager.CheckPasswordAsync(user, loginRequest.Password))
            {
                // Call my service class method
                var token = await _tokenGenerator.GenerateToken(user);

                var cookieOptions = new CookieOptions
                {
                    HttpOnly = true, // HttpOnly is a security flag added to a browser cookie that prevents client-side scripts—specifically JavaScript—from accessing or manipulating it.
                    Secure = true, // True to make cookie to be accepted only from HTTPS, otherwise backend will not accept cookie
                    Domain = "localhost",
                    Path = "/",
                    Expires = DateTime.UtcNow.AddHours(cookieExpirationHours),
                    IsEssential = true,
                    SameSite = SameSiteMode.None, // SameSite = SameSiteMode.Strict o Lax to avoid Cross-Site Request Forgery (CSRF), I am using None to avoid problems from having backend and frontend as cross-origin
                };
                Response.Cookies.Append("token", token, cookieOptions);
                Response.Cookies.Append("email", loginRequest.Email);

                var emailForResponse = user.Email;
                var roles = await _userManager.GetRolesAsync(user);

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

                //return Ok(new { authenticated = true });
                //return Ok(new {token});

                return Ok(new
                {
                    Email = emailForResponse,
                    Roles = roles,
                    IsAuthenticated = true,
                    authenticated = true
                });
                
            }

            return Unauthorized();

        }



        [HttpPost("logout")]
        public IActionResult Logout()
        {

            var cookieOptions = new CookieOptions
            {
                Expires = DateTime.Now.AddDays(-1),
                IsEssential = false
            };

            Response.Cookies.Append("token", "", cookieOptions);
            Response.Cookies.Append("email", "");

            return Ok("Logout successful");
        }


        // Verify token
        //[HttpGet("authenticated")]
        //public IActionResult Authenticated()
        //{
        //    // var token2 = Request.Cookies["token2"]; // this is another way of doing the following line
        //    var token = HttpContext.Request.Cookies["token"];

        //    if(token.IsNullOrEmpty())
        //    {
        //        return Unauthorized();
        //    }

        //    var tokenHandler = new JwtSecurityTokenHandler();
        //    var validationParameters = new TokenValidationParameters
        //    {
        //        ValidateIssuer = true,
        //        ValidateAudience = true,
        //        ValidateLifetime = true,
        //        ValidateIssuerSigningKey = true,
        //        ValidIssuer = "MovieTheaterWS-v2", //check this
        //        ValidAudience = "movie-theater-web-app", //check this
        //        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("H7YVZWs1TnxVF8tCOCLF2/RJRy0FK3Hk")) //check this
        //    };

        //    try
        //    {
        //        var claimsPrincipal = tokenHandler.ValidateToken(token, validationParameters, out var securityToken);

        //        if(securityToken is JwtSecurityToken jwtSecurityToken)
        //        {
        //            var expirationDate = jwtSecurityToken.ValidTo;

        //            if(expirationDate < DateTime.UtcNow)
        //            {
        //                // Token has expired
        //                return Unauthorized();
        //            }


        //            LoginResponse loginResponse = new LoginResponse
        //            {
        //                Authenticated = true,
        //                Email = HttpContext.Request.Cookies["email"]
        //            };

        //            //return Ok(new { authenticated = true });
        //            return Ok(loginResponse);
        //        }
        //        else
        //        {
        //            // Token is not JWT
        //            return Unauthorized();
        //        }
        //    }
        //    catch(SecurityTokenException ex)
        //    {
        //        // Token is invalid
        //        return Unauthorized();
        //    }

        //}

        [Authorize] // If cookie is valid, it enters. If not, it returns error 401
        [HttpGet("authenticated")]
        public IActionResult Authenticated()
        {
            // You do not need to read the cookie manually with HttpContext.Request.Cookies["token"]
            // The middleware already did it for you and filled the object User.

            return Ok(new
            {
                Email = User.FindFirstValue(ClaimTypes.Email),
                //Role = User.FindFirstValue(ClaimTypes.Role),
                Roles = User.FindAll(ClaimTypes.Role).Select(c => c.Value).ToList(),
                IsAuthenticated = true,
                Authenticated = true
            });
        }
    }
}
