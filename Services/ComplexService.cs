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
        public async Task<Complex> ProcessAsync(ComplexCreationDTO complexDTO)
        {
            Complex newComplex = new Complex
            {
                Name = complexDTO.Name
            };

            // Wait asynchronously for the repository to finish saving
            await _complexRepository.SaveAsync(newComplex);

            return newComplex;
        }
    }
}
