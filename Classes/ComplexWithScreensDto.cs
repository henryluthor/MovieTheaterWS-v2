namespace MovieTheaterWS_v2.Classes
{
    public class ComplexWithScreensDto
    {
        public int IdComplex { get; set; }
        public required string Name { get; set; } = string.Empty;

        // Here we save the list of your screens using the flat DTO
        public List<ScreenDto> Screens { get; set; } = new ();
    }
}
