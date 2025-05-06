namespace MovieTheaterWS_v2.Classes
{
    public class LoginResponse
    {
        public bool Success {  get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; } = null!;
        public int? IdRole { get; set; }
    }
}
