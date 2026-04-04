namespace MovieTheaterWS_v2.Classes
{
    public class UserRegistrationBaseDTO
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }

        // The exclamation mark after null says to the compiler "Trust me that when the application runs this property will NOT be null
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
    }
}
