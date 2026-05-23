using MovieTheaterWS_v2.Models;
using System.Threading.Tasks;

namespace MovieTheaterWS_v2.Repositories
{
    public class MovieRepository
    {
        private readonly MovietheaterContext _context;

        public MovieRepository(MovietheaterContext context)
        {
            _context = context;
        }


        public void Add(Movie movie)
        {
            _context.Movies.Add(movie);
        }


        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
