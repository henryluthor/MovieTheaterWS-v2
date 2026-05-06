using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieTheaterWS_v2.Classes;
using MovieTheaterWS_v2.Models;

namespace MovieTheaterWS_v2.Controllers
{
    [Authorize(Roles = "Admin")]
    [Route("api/[controller]")]
    [ApiController]
    public class ComplexController : ControllerBase
    {
        private readonly MovietheaterContext _context;
        //private readonly IMapper _mapper;

        public ComplexController(MovietheaterContext context)
        {
            _context = context;
            //_mapper = mapper;
        }

        // GET: api/<SystemUserController>
        [HttpGet]
        public async Task<List<Complex>> Get()
        {
            return await _context.Complexes.ToListAsync();
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] ComplexCreationDTO complexCreationDTO)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            
            var complexNameSearched = _context.Complexes.Where(c => c.Name == complexCreationDTO.Name);

            // Check if name already exists
            if (complexNameSearched.Any()) return BadRequest( new { message = "There is a complex with that name already." });

            try
            {
                Complex complex = new Complex
                {
                    Name = complexCreationDTO.Name,
                };

                //Complex complex = _mapper.Map<Complex>(complexCreationDTO);

                _context.Complexes.Add(complex);

                await _context.SaveChangesAsync();

                return Ok(new { message = "Complex registered successfully." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "There was an error while trying to register the complex. " + ex.Message});
            }

        }
        
    }
}
