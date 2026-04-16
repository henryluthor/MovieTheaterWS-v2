using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MovieTheaterWS_v2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoleController : ControllerBase
    {
        private readonly RoleManager<IdentityRole> _roleManager;

        public RoleController(RoleManager<IdentityRole> roleManager)
        {
            _roleManager = roleManager;
        }

        // GET: api/<RoleController>
        [HttpGet]
        public async Task<List<IdentityRole>> Get()
        {
            return await _roleManager.Roles.ToListAsync();
        }

        //public IActionResult Index()
        //{
        //    return View();
        //}
    }
}
