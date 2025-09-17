using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieTheaterWS_v2.Models;

namespace MovieTheaterWS_v2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ComplexMovieController : ControllerBase
    {
        private readonly MovietheaterContext _context;

        public ComplexMovieController(MovietheaterContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<List<ComplexMovie>> Get()
        {
            //return View();
            return await _context.ComplexMovies.ToListAsync();
        }

        [HttpGet("{idComplex}")]
        public async Task<List<ComplexMovie>> GetMoviesByComplex(int idComplex)
        {
            var allComplexMovies = from cm in _context.ComplexMovies select cm;

            return await allComplexMovies.Where(cm => cm.IdComplex == idComplex).ToListAsync();
        }
    }
}
