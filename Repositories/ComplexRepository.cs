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
        
        public void Add(Complex complex)
        {
            //_context.Add(complex);
            // Previous line is valid, next line is more specific so EF does not need to look the entity in the type
            _context.Complexes.Add(complex);
        }

        // Searches the record and keeps it under tracking
        public async Task<Complex?> GetByIdAsync(int id)
        {
            return await _context.Complexes.FindAsync(id);
        }


        // Returns Task because it is an operation that does not return an immediate value
        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
