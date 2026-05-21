using MovieTheaterWS_v2.Models;
using System.Threading.Tasks;

namespace MovieTheaterWS_v2.Repositories
{
    public class ScreenRepository
    {
        private readonly MovietheaterContext _context;

        public ScreenRepository(MovietheaterContext context)
        {
            _context = context;
        }

        public void Add(Screen screen)
        {
            _context.Screens.Add(screen);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
