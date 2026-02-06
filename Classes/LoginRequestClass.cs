namespace MovieTheaterWS_v2.Classes
{
    public class LoginRequestClass
    {
        public string Email {  get; set; }
        public string Password { get; set; }

        public LoginRequestClass()
        {
            Email = string.Empty;
            Password = string.Empty;
        }
    }
}
