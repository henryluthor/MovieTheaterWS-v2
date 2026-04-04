namespace MovieTheaterWS_v2.Classes
{
    public class AdminRegistrationDTO: UserRegistrationBaseDTO
    {
        string RoleName { get; set; } = "Customer";
    }
}
