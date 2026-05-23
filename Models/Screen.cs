using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MovieTheaterWS_v2.Models
{
    public class Screen
    {
        [Key]
        public int IdScreen { get; set; }

        [Required]
        [StringLength(50)]
        public required string Name { get; set; }

        // Foreign Key
        //[ForeignKey("Complex")] // Commented since doing it with Fluent API
        public int IdComplex { get; set; }

        // Navigation Property
        public Complex? Complex { get; set; }
        
        public ICollection<Seat> Seats { get; set; } = new List<Seat>();
    }
}
