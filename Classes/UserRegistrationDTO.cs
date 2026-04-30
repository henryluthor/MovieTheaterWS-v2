namespace MovieTheaterWS_v2.Classes
{
    public class UserRegistrationDTO: UserRegistrationBaseDTO
    {
        public List<string> Roles { get; set; } = ["Customer"];
    }
}
