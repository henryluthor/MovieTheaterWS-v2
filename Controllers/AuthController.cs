using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using MovieTheaterWS_v2.Classes;

namespace MovieTheaterWS_v2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private int cookieExpirationMinutes = 1;

        // Generate token and create cookie to store token
        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequest loginRequest)
        {
            // Authenticate user
            var token = GenerateToken();
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                Expires = DateTime.UtcNow.AddMinutes(cookieExpirationMinutes)
            };
            Response.Cookies.Append("token", token, cookieOptions);
            return Ok();
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
            if(token != null)
            {
                // Verify token
                //var email = GetEmailFromToken(token);
                //return Ok(new { authenticated = true, email });
                return Ok(new { authenticated = true });
            }
            else
            {
                return Ok(new { Authenticated = false });
            }
        }
    }
}
