using MovieTheaterWS_v2.Classes;
using MovieTheaterWS_v2.Models;
using MovieTheaterWS_v2.Repositories;

namespace MovieTheaterWS_v2.Services
{
    public class ScreenService
    {
        private readonly ScreenRepository _screenRepository;

        public ScreenService(ScreenRepository screenRepository)
        {
            _screenRepository = screenRepository;
        }

        public async Task<Screen> CreateAsync(ScreenCreationDto dto)
        {
            // Map
            Screen newScreen = new Screen
            {
                Name = dto.Name,
                IdComplex = dto.IdComplex,
            };

            _screenRepository.Add(newScreen);
            await _screenRepository.SaveChangesAsync();

            return newScreen;
        }
    }
}
