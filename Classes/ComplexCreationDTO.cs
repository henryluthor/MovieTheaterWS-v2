using System.ComponentModel.DataAnnotations;

namespace MovieTheaterWS_v2.Classes
{
    public class ComplexCreationDTO
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "The complex name is required and cannot be empty.")]
        [StringLength(50, ErrorMessage = "The complex name must have a maximum of 50 characters")]
        public required string Name { get; set; }
    }
}
