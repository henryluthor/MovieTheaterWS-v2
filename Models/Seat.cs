using System.ComponentModel.DataAnnotations;

namespace MovieTheaterWS_v2.Models
{
    public class Seat
    {
        [Key]
        public int IdSeat { get; set; }        
        public required string Row { get; set; }
        public required int Column { get; set; }

        public int IdScreen { get; set; }
        public Screen? Screen { get; set; }
    }
}
