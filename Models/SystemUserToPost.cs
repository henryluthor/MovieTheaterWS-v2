namespace MovieTheaterWS_v2.Models
{
    public class SystemUserToPost
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
        public int IdRole { get; set; }
    }
}
