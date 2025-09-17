using System;
using System.Collections.Generic;

namespace MovieTheaterWS_v2.Models;

public partial class ComplexMovie
{
    public int IdComplex { get; set; }

    public int IdMovie { get; set; }

    public virtual Movie IdMovieNavigation { get; set; } = null!;
}
