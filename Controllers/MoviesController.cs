﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieTheaterWS_v2.Classes;
using MovieTheaterWS_v2.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MovieTheaterWS_v2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MoviesController : ControllerBase
    {
        private readonly MovietheaterContext _context;

        public MoviesController(MovietheaterContext context)
        {
            _context = context;
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
        public void Post([FromBody] string value)
        {
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
