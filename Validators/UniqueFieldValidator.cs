using Microsoft.EntityFrameworkCore;
using MovieTheaterWS_v2.Classes;
using MovieTheaterWS_v2.Models;

namespace MovieTheaterWS_v2.Validators
{
    public class UniqueFieldValidator
    {
        private readonly MovietheaterContext _context;

        public UniqueFieldValidator(MovietheaterContext context)
        {
            _context = context;
        }

        public async Task<bool> IsNameDuplicatedAsync<T>(string name, int? currentId = null) where T : class, INamedEntity
        {
            if (string.IsNullOrWhiteSpace(name)) return false;

            // Converting to lowercase for uppercase insensitive validation
            var query = _context.Set<T>().Where(e => e.Name.ToLower() == name.ToLower());

            if (currentId.HasValue)
            {
                query = query.Where(e => EF.Property<int>(e, "Id") != currentId.Value);
            }

            return await query.AnyAsync();
        }
    }
}
