namespace MovieTheaterWS_v2.Models
{
    public class Showing
    {
        public required DateTimeOffset Showtime { get; set; }
        public required int IdScreen { get; set; }
        public required int IdMovie { get; set; }
    }
}
