using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ApiFaceUnah.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly DBContext _context;

        public UsersController(DBContext context)
        {
            _context = context;
        }

        // Get: api/users
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Models.Users>>> GetUsers()
        {
            var users = await _context.Users.ToListAsync();

            if(users == null)
            {
                return NotFound(
                        new { message = "No users found"}
                    ); 
            }

            return Ok(users);
        }

        // Get: api/users/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Models.Users>> GetUsersId(int id)
        {
            var user = await _context.Users.FindAsync(id);

            if(user == null)
            {
                return NotFound(
                    new { message = "User not found"}
                    );
            }

            return Ok(user);
        }

        // POST: api/users
        [HttpPost]
        public async Task<ActionResult<Models.Users>> CreateUser(Models.Users user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetUsersId), new { id = user.Id }, user);
        }
    }
}
