namespace MovieTheaterWS_v2.Classes
{
    public class UserUpdateDTO
    {
        public required string Email { get; set; }
        public required List<string> Roles { get; set; }
    }
}
