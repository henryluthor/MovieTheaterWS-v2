using MovieTheaterWS_v2.Models;
using System.Threading.Tasks;

namespace MovieTheaterWS_v2.Repositories
{
    public class ComplexRepository
    {
        private readonly MovietheaterContext _context;

        public ComplexRepository(MovietheaterContext context)
        {
            _context = context;
        }

        // Returns Task because it is an operation that does not return an immediate value
        public async Task SaveAsync(Complex complex)
        {
            await _context.AddAsync(complex);            
            await _context.SaveChangesAsync();
        }
    }
}
