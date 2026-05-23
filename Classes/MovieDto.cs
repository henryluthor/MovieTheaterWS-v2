namespace MovieTheaterWS_v2.Classes
{
    public class MovieDto
    {
        public required string Title { get; set; }
        public int? Year { get; set; }
        public required int Runtime { get; set; }
        public string? Genre { get; set; }
        public string? IdImdb { get; set; }
    }
}
