using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MovieTheaterWS_v2.Classes
{
    public class LoginResponse
    {
        public bool Authenticated { get; set; } = false;
        public string? Email { get; set; } = null!;
    }
}
