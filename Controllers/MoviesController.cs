using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieTheaterWS_v2.Classes;
using MovieTheaterWS_v2.Models;
using MovieTheaterWS_v2.Services;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MovieTheaterWS_v2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MoviesController : ControllerBase
    {
        private readonly MovietheaterContext _context;
        private readonly MovieService _movieService;

        public MoviesController(MovietheaterContext context, MovieService movieService)
        {
            _context = context;
            _movieService = movieService;
        }

        // GET: api/<MoviesController>
        [HttpGet]
        //original line
        //public IEnumerable<string> Get()
        public async Task<List<Movie>> Get()
        {
            //original line
            //return new string[] { "value1", "value2" };
            return await _context.Movies.ToListAsync();
        }

        // GET api/<MoviesController>/5
        [HttpGet("{id}")]
        // original line
        // public string Get(int id)
        public async Task<GenericResponse<Movie>> Get(int id)
        {
            //return "value";
            //return await _context.Movies.FindAsync(id);
            var genResponse = new GenericResponse<Movie>();
            genResponse.Data = await _context.Movies.FindAsync(id);
            if(genResponse.Data == null)
            {
                genResponse.Message = "Movie not found.";
            }
            return genResponse;
        }

        // POST api/<MoviesController>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] MovieDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            try
            {
                Movie movie = await _movieService.CreateAsync(dto);

                return CreatedAtAction
                    (
                    nameof(Get),
                    new {id = movie.IdMovie},
                    new {movie, message = "Movie registered successfully." }
                    );
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An internal error occurred on the server.", detail = ex.Message});
            }
        }

        // PUT api/<MoviesController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<MoviesController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
