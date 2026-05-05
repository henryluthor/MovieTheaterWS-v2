using System.ComponentModel.DataAnnotations;

namespace MovieTheaterWS_v2.Classes
{
    public class ComplexCreationDTO
    {
        [Required]
        [StringLength(50)]
        public required string Name { get; set; }
    }
}
