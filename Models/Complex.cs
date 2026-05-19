using System;
using System.Collections.Generic;

namespace MovieTheaterWS_v2.Models;

public partial class Complex
{
    public int IdComplex { get; set; }

    public required string Name { get; set; }

    public virtual ICollection<Movie> Movies { get; set; } = new List<Movie>();
}
