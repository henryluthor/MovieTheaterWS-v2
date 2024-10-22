using System;
using System.Collections.Generic;

namespace MovieTheaterWS_v2.Models;

public partial class Movie
{
    public int Id { get; set; }

    public string Title { get; set; } = null!;

    /// <summary>
    /// Year of release of the movie.
    /// </summary>
    public int? Year { get; set; }

    /// <summary>
    /// Runtime in minutes.
    /// </summary>
    public int? Runtime { get; set; }

    public string? Genre { get; set; }
}
