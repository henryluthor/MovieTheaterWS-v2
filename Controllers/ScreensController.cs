using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieTheaterWS_v2.Classes;
using MovieTheaterWS_v2.Models;
using MovieTheaterWS_v2.Services;

namespace MovieTheaterWS_v2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ScreensController : ControllerBase
    {
        private readonly MovietheaterContext _context;
        private readonly ScreenService _screenService;

        public ScreensController(MovietheaterContext context, ScreenService screenService)
        {
            _context = context;
            _screenService = screenService;
        }

        [HttpGet]
        public async Task<List<Screen>> Get()
        {
            return await _context.Screens.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var screen = await _context.Screens.FindAsync(id);

            if (screen == null) return NotFound();

            return Ok(screen);
        }


        [HttpPost]
        public async Task<IActionResult> Post([FromBody] ScreenDto dto)
        {
            if(!ModelState.IsValid) return BadRequest(ModelState);

            try
            {
                Screen screen = await _screenService.CreateAsync(dto);

                return CreatedAtAction
                    (
                    nameof(Get),
                    new {id = screen.IdScreen},
                    new { screen, message = "Screen registered successfully." }
                    );
            }
            catch(DbUpdateException ex)
            {
                if (IsDuplicationError(ex))
                {
                    return Conflict(new
                    {
                        mensaje = $"There is already a screen with the name '{dto.Name}' in this complex."
                    });
                }

                // If it is another type of database error, relaunch or handle it generally
                return StatusCode(500, "An unexpected error occurred in the database.");
            }
            catch(ArgumentException ex)
            {
                // Catches invalid business data errors
                return BadRequest("There was an error while trying to register the screen. " + ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An internal error occurred on the server.", detail = ex.Message });
            }
        }

        public bool IsDuplicationError(DbUpdateException ex)
        {
            if(ex.InnerException == null) return false;

            string message = ex.InnerException.Message.ToLower();

            // SQL Server: Error 2601 (Index) o 2627 (Unique constraint)
            if (message.Contains("unique index") || message.Contains("duplicate key"))
            {
                return true;
            }

            // PostgreSQL: Código de error 23505 (unique_violation)
            if (message.Contains("23505"))
            {
                return true;
            }

            // MySQL: Código de error 1062 (Duplicate entry)
            if (message.Contains("1062") || message.Contains("duplicate entry"))
            {
                return true;
            }

            return false;
        }
    }
}
