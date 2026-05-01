using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MovieTheaterWS_v2.Classes;
using MovieTheaterWS_v2.Models;
using System.Security.Claims;
using System.Threading.Tasks;
//using System.Security.Cryptography;
//using System.Text;

namespace MovieTheaterWS_v2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SystemUserController : ControllerBase
    {
        private readonly MovietheaterContext _context;
        private readonly UserManager<User> _userManager;

        public SystemUserController(MovietheaterContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: api/<SystemUserController>
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<List<UserResponseDTO>> Get()
        {
            //return await _context.Users.ToListAsync(); // This is not right because it returns everything, including sensitive information
            return await _context.Users.Select(u => new UserResponseDTO { Id = u.Id, Email = u.Email! }).ToListAsync();
        }


        [Authorize]
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            if (string.IsNullOrEmpty(id)) return NotFound();

            // Check if user who made the request has role Admin
            var isRequestUserAdmin = User.IsInRole("Admin");

            // Check id of user who made the request
            //var requestUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            // Another way of doing the same as the line above
            var requestUserId = _userManager.GetUserId(User);

            var isSameUser = requestUserId == id;

            // If user who made the request has role Admin or has the same id that is being consulted search data
            if (isRequestUserAdmin || isSameUser)
            {
                var userToLookFor = await _userManager.FindByIdAsync(id);

                if (userToLookFor == null) return NotFound();

                //List<string> userToLookForRoles = (await _userManager.GetRolesAsync(userToLookFor)).ToList();
                List<string> userToLookForRoles = [.. await _userManager.GetRolesAsync(userToLookFor)];

                UserResponseDTO userResponseDTO = new UserResponseDTO
                {
                    Id = id,
                    Email = userToLookFor.Email!,
                    Roles = userToLookForRoles
                };

                return Ok(userResponseDTO);
            }

            return Forbid();
            
        }

        

        // Deprecated since using AspNetCore.Identity
        // POST: api/<SystemUserController>
        //[HttpPost]
        ////[Authorize(Policy = "AdminOnly")]
        //[Authorize(Roles = "Admin")]
        //public async Task<GenericResponse<User>> PostOld(SystemUserToPost systemUserToPost)
        //{
        //    var genRensponse = new GenericResponse<User>();

        //    var allSystemUsers = from s in _context.Users select s;
        //    allSystemUsers = allSystemUsers.Where(s => s.Email == systemUserToPost.Email);

        //    // Check if email is already registered
        //    if (allSystemUsers.Any())
        //    {
        //        genRensponse.Message = "That email is registered already.";
        //    }
        //    else
        //    {
        //        try
        //        {
        //            // Hash userToLookFor password
        //            SHA512 hashSvc = SHA512.Create();
        //            byte[] hash = hashSvc.ComputeHash(Encoding.UTF8.GetBytes(systemUserToPost.Password));

        //            User systemUser = new User();
        //            systemUser.FirstName = systemUserToPost.FirstName;
        //            systemUser.LastName = systemUserToPost.LastName;
        //            systemUser.Email = systemUserToPost.Email;
        //            systemUser.PasswordHash = BitConverter.ToString(hash).Replace("-", "");
        //            //systemUser.IdRole = userToPost.IdRole;

        //            _context.Users.Add(systemUser);
        //            await _context.SaveChangesAsync();

        //            genRensponse.Message = "User registered successfully.";
        //            genRensponse.Data = systemUser;
        //        }
        //        catch (Exception ex)
        //        {
        //            genRensponse.Message = "There was an error while trying to register the userToLookFor. " + ex.Message;
        //        }
        //    }

        //    return genRensponse;
        //}


        //[AllowAnonymous]
        [HttpPost("register-customer")]
        public async Task<IActionResult> RegisterCustomer([FromBody] CustomerRegistrationDTO userToPost)
        {
            var user = new User
            {
                UserName = userToPost.Email,
                Email = userToPost.Email,
                FirstName = userToPost.FirstName,
                LastName = userToPost.LastName,
                IsDeleted = false
            };

            // It is not necessary to manually hash the password, CreateAsync does it
            var result = await _userManager.CreateAsync(user, userToPost.Password);

            if (result.Succeeded)
            {
                // User created successfully
                // Forced role assignation from server, ignoring any attempt from client
                await _userManager.AddToRoleAsync(user, "Customer");
                return Ok();
            }
            //return BadRequest(result.Errors.Select(e => e.Description));
            return BadRequest(result.Errors);
        }



        [Authorize(Roles = "Admin")]
        [HttpPost("create-user")]
        public async Task<IActionResult> CreateUser([FromBody] UserRegistrationDTO userToPost)
        {
            var user = new User
            {
                UserName = userToPost.Email,
                Email = userToPost.Email,
                FirstName = userToPost.FirstName,
                LastName = userToPost.LastName,
                IsDeleted = false
            };

            // It is not necessary to manually hash the password, CreateAsync does it
            var result = await _userManager.CreateAsync(user, userToPost.Password);
            if (result.Succeeded)
            {
                // User created successfully
                // Here you trust the model because only an Admin reached this point
                await _userManager.AddToRolesAsync(user, userToPost.Roles);
                return Ok();
            }
            //return BadRequest(result.Errors.Select(e => e.Description));
            return BadRequest(result.Errors);
        }



        [Authorize]
        [HttpPut("update-customer/{id}")]
        public async Task<IActionResult> UpdateCustomer(string id, [FromBody] UserUpdateDTO userUpdateDTO)
        {
            if(string.IsNullOrEmpty(id)) return NotFound();

            // Check if user who made the request has role Admin
            var isRequestUserAdmin = User.IsInRole("Admin");

            // Check id of user who made the request
            //var requestUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            // Another way of doing the same as the line above
            var requestUserId = _userManager.GetUserId(User);

            var isSameUser = requestUserId == id;

            // If user who made the request has role Admin or has the same id that is being consulted update user
            if (isRequestUserAdmin || isSameUser)
            {
                var user = await _userManager.FindByIdAsync(id);

                if (user == null) return NotFound($"User with ID {id} not found.");

                user.Email = userUpdateDTO.Email;

                var result = await _userManager.UpdateAsync(user);
                // To change password
                // _userManager.ChangePasswordAsync(user, oldPassword, newPassword).
                // to update user roles
                // _userManager.AddToRoleAsync o _userManager.RemoveFromRoleAsync

                if (result.Succeeded)
                {
                    return Ok(new { message = "User updated successfully." });
                }

                return BadRequest(result.Errors);
            }

            return Forbid();
            
        }


        [Authorize(Roles = "Admin")]
        [HttpPut("update-user/{id}")]
        public async Task<IActionResult> UpdateUser(string id, [FromBody] UserUpdateDTO userUpdateDTO)
        {
            if (string.IsNullOrEmpty(id)) return NotFound();

            var user = await _userManager.FindByIdAsync(id);

            if (user == null) return NotFound();

            user.Email = userUpdateDTO.Email;

            var result = await _userManager.UpdateAsync(user);

            if (result.Succeeded)
            {
                return Ok(new { message = "User updated successfully." });
            }

            return BadRequest(result.Errors);
        }


        [Authorize]
        [HttpDelete("delete-customer/{id}")]
        public async Task<IActionResult> DeleteCustomer(string id)
        {
            if (string.IsNullOrEmpty(id)) return NotFound();

            // Check if user who made the request has role Admin
            var isRequestUserAdmin = User.IsInRole("Admin");

            // Check id of user who made the request
            //var requestUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            // Another way of doing the same as the line above
            var requestUserId = _userManager.GetUserId(User);

            var isSameUser = requestUserId == id;

            // If user who made the request has role Admin or has the same id that is being consulted delete user
            if (isRequestUserAdmin || isSameUser)
            {
                var user = await _userManager.FindByIdAsync(id);

                if (user == null) return NotFound();

                user.IsDeleted = true;
                // IMPORTANT: Block access
                // This prevent the user from logging in
                user.LockoutEnabled = true;
                user.LockoutEnd = DateTimeOffset.MaxValue;

                var result = await _userManager.UpdateAsync(user);

                if(result.Succeeded)
                {
                    return NoContent();
                }

                return BadRequest(result.Errors);
            }

            return Forbid();

        }


        [Authorize(Roles = "Admin")]
        [HttpDelete("delete-user/{id}")]
        public async Task<IActionResult> DeleteUser(string id)
        {
            if (string.IsNullOrEmpty(id)) return NotFound();

            var user = await _userManager.FindByIdAsync(id);

            if (user == null) return NotFound();

            user.IsDeleted = true;
            // IMPORTANT: Block access
            // This prevent the user from logging in
            user.LockoutEnabled = true;
            user.LockoutEnd = DateTimeOffset.MaxValue;

            var result = await _userManager.UpdateAsync(user);

            if (result.Succeeded)
            {
                return NoContent();
            }

            return BadRequest(result.Errors);
        }



        [Authorize(Roles = "Admin")]
        [HttpPost("restore/{id}")]
        public async Task<IActionResult> Restore (string id)
        {
            if (string.IsNullOrEmpty(id)) return NotFound();

            var user = await _context.Users.IgnoreQueryFilters().FirstOrDefaultAsync(u => u.Id == id);

            if (user == null) return NotFound("User does not exist in database.");

            if (!user.IsDeleted) return BadRequest("User is already active.");

            // Restore fields
            user.IsDeleted = false;
            user.LockoutEnd = null; // Remove access block

            var result = await _userManager.UpdateAsync(user);

            if (result.Succeeded)
            {
                return Ok("User restored successfully.");
            }

            return BadRequest(result.Errors);
        }

    }
}
