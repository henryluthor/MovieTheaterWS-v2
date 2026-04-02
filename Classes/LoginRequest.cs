namespace MovieTheaterWS_v2.Classes
{
    public class LoginRequest
    {
        public string Email {  get; set; }
        public string Password { get; set; }

        public LoginRequest()
        {
            Email = string.Empty;
            Password = string.Empty;
        }
    }
}
