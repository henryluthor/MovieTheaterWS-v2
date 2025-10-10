using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieTheaterWS_v2.Classes;
using MovieTheaterWS_v2.Models;

namespace MovieTheaterWS_v2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ComplexController : ControllerBase
    {
        private readonly MovietheaterContext _context;

        public ComplexController(MovietheaterContext context)
        {
            _context = context;
        }

        // GET: api/<SystemUserController>
        [HttpGet]
        public async Task<List<Complex>> Get()
        {
            return await _context.Complexes.ToListAsync();
        }

        [HttpPost]
        public async Task<GenericResponse<Complex>> Post([FromBody] string complexName)
        {
            GenericResponse<Complex> genResponse = new GenericResponse<Complex>();
            var complexNameSearched = _context.Complexes.Where(c => c.Name == complexName);

            // Check if name already exists
            if (complexNameSearched.Any())
            {
                genResponse.Message = "There is a complex with that name already.";
            }
            else
            {
                try
                {
                    Complex complex = new Complex();
                    complex.Name = complexName;
                    _context.Complexes.Add(complex);
                    await _context.SaveChangesAsync();

                    genResponse.Message = "Complex registered successfully.";
                    genResponse.Data = complex;
                }
                catch (Exception ex)
                { 
                    genResponse.Message = "There was an error while trying to register the complex. " + ex.Message;
                }                
            }

            return genResponse;
        }
        
    }
}
