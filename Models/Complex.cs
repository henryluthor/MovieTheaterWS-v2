using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MovieTheaterWS_v2.Models;

public partial class Complex
{
    //[Key] // commented since doing it with Fluent API
    public int IdComplex { get; set; }

    [Required]
    public required string Name { get; set; }

    public virtual ICollection<Movie> Movies { get; set; } = new List<Movie>();

    public ICollection<Screen> Screens { get; set; } = new List<Screen>();
}
