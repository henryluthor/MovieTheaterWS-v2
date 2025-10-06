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

        //This method only returns IDs, not very user friendly
        //[HttpGet("{idComplex}")]
        //public async Task<List<ComplexMovie>> GetMoviesByComplex(int idComplex)
        //{
        //    var allComplexMovies = from cm in _context.ComplexMovies select cm;

        //    return await allComplexMovies.Where(cm => cm.IdComplex == idComplex).ToListAsync();
        //}

        // This method need protection against SQL injection
        [HttpGet("{idComplex}")]
        public async Task<List<Movie>> GetMoviesByComplex(int idComplex)
        {
            //int idComplexToSearch = idComplex;
            var moviesByComplex = await _context.Movies.FromSqlInterpolated(
                $@"select m.idMovie, m.Title, m.Year, m.Runtime, m.Genre, m.IMDBId 
from ComplexMovie cm 
join Movie m on cm.idMovie = m.idMovie 
where cm.idComplex = {idComplex}"
                ).ToListAsync();
            
            return moviesByComplex;
        }
    }
}
