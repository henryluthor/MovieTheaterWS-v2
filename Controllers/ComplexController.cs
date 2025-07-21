using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
        
    }
}
