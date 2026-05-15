using MovieTheaterWS_v2.Classes;
using MovieTheaterWS_v2.Models;
using MovieTheaterWS_v2.Repositories;

namespace MovieTheaterWS_v2.Services
{
    public class ComplexService
    {
        private readonly ComplexRepository _complexRepository;

        public ComplexService(ComplexRepository complexRepository)
        {
            _complexRepository = complexRepository;
        }

        // Returns Task<Complex> because upon completion it will return a Complex object
        public async Task<Complex> CreateAsync(ComplexCreationDto complexDto)
        {
            Complex newComplex = new Complex
                {
                    Name = complexDto.Name
                };
            
            _complexRepository.Add(newComplex);

            // Wait asynchronously for the repository to finish saving
            await _complexRepository.SaveChangesAsync();

            return newComplex;
        }

        public async Task UpdateAsync(int id, ComplexUpdateDto complexUpdateDto)
        {
            // The repository searches for the record (EF Core starts tracking it)
            var existingComplex = await _complexRepository.GetByIdAsync(id);

            if (existingComplex == null)
            {
                throw new KeyNotFoundException($"Complex with ID {id} not found.");
            }

            // Modifying properties in memory
            existingComplex.Name = complexUpdateDto.Name;

            // Confirming the changes. EF Core detects the modification automatically
            await _complexRepository.SaveChangesAsync();
        }
    }
}
