using MovieTheaterWS_v2.Classes;
using MovieTheaterWS_v2.Models;
using System.Threading.Tasks;

namespace MovieTheaterWS_v2.Services
{
    public class MovieService
    {
        private readonly MovietheaterContext _context;

        public MovieService(MovietheaterContext context)
        {
            _context = context;
        }

        public async Task<Movie> CreateAsync(MovieDto dto)
        {
            var newMovie = new Movie
            {
                Title = dto.Title,
                Year = dto.Year,
                Runtime = dto.Runtime,
                Genre = dto.Genre,
                IdImdb = dto.IdImdb
            };

            _context.Movies.Add(newMovie);

            await _context.SaveChangesAsync();

            return newMovie;
        }
    }
}
