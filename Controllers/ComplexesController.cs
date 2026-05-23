using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieTheaterWS_v2.Classes;
using MovieTheaterWS_v2.Models;
using MovieTheaterWS_v2.Services;
using MovieTheaterWS_v2.Validators;

namespace MovieTheaterWS_v2.Controllers
{
    [Authorize(Roles = "Admin")]
    [Route("api/[controller]")]
    [ApiController]
    public class ComplexesController : ControllerBase
    {
        private readonly MovietheaterContext _context;
        private readonly UniqueFieldValidator _uniqueFieldValidator;
        private readonly ComplexService _complexService;
        

        public ComplexesController(
            MovietheaterContext context,
            UniqueFieldValidator uniqueFieldValidator,
            ComplexService complexService
            )
        {
            _context = context;
            _uniqueFieldValidator = uniqueFieldValidator;
            _complexService = complexService;
        }

        // GET: api/<SystemUserController>
        [HttpGet]
        public async Task<List<Complex>> Get()
        {
            return await _context.Complexes.ToListAsync();
        }


        [HttpGet("{id}")]
        // The following 2 attributes help me keep Swagger automatic documentation benefits
        [ProducesResponseType(typeof(Complex), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Get(int id)
        {
            var complex = await _context.Complexes.FindAsync(id);

            if (complex == null) return NotFound();

            return Ok(complex);
        }


        [HttpPost]
        public async Task<IActionResult> Post([FromBody] ComplexCreationDto complexCreationDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            bool isDuplicate = await _uniqueFieldValidator.IsNameDuplicatedAsync<Complex>(complexCreationDto.Name);

            if (isDuplicate)
            {
                //ModelState.AddModelError("Name", "There is a complex with that name already.");
                //return BadRequest(ModelState);
                return BadRequest( new { message = "There is a complex with that name already." });
            }
            
            // Deprecated code since using validator class, kept for reference
            //var complexNameSearched = _context.Complexes.Where(c => c.Name == complexCreationDto.Name);
            //// Check if name already exists
            //if (complexNameSearched.Any()) return BadRequest( new { message = "There is a complex with that name already." });

            try
            {
                Complex complex = await _complexService.CreateAsync(complexCreationDto);

                //return Ok(new { message = "Complex registered successfully." });
                // Returns HTTP 201 (Created) which is good practice when saving
                return CreatedAtAction
                    (
                    nameof(Get),
                    new {id = complex.IdComplex},
                    new { complex, message = "Complex registered successfully." }
                    );
            }
            catch (ArgumentException ex)
            {
                // Catches invalid business data errors
                return BadRequest(new { message = "There was an error while trying to register the complex. " + ex.Message});
            }
            catch (Exception ex)
            {
                // Catches unexpected errors (database crashes, code bugs)
                // Here you should use a Logger (for example: _logger.LogError(ex, "Error..."))
                return StatusCode(500, new { message = "An internal error occurred on the server.", detail = ex.Message });

            }

        }


        [HttpPut("{id}")]
        public async Task<IActionResult> Put (int id, [FromBody] ComplexUpdateDto complexUpdateDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            bool isDuplicate = await _uniqueFieldValidator.IsNameDuplicatedAsync<Complex>(complexUpdateDto.Name);

            if (isDuplicate)
            {
                return BadRequest(new { message = "There is a complex with that name already." });
            }

            try
            {
                await _complexService.UpdateAsync(id, complexUpdateDto);
                return NoContent();
            }
            catch(ArgumentException ex)
            {
                // Catches invalid business data errors
                return BadRequest(new { message = "There was an error while trying to update the complex. " + ex.Message });
            }
            catch(Exception ex)
            {
                // Catches unexpected errors (database crashes, code bugs)
                // Here you should use a Logger (for example: _logger.LogError(ex, "Error..."))
                return StatusCode(500, new { message = "An internal error occurred on the server", detail = ex.Message });
            }


        }
        
    }
}
