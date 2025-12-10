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

            return users == null
                ? (ActionResult<IEnumerable<Models.Users>>)NotFound(
                        new { message = "No hay usuarios" }
                    )
                : (ActionResult<IEnumerable<Models.Users>>)Ok(users);
        }

        // Get: api/users/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Models.Users>> GetUsersId(int id)
        {
            var user = await _context.Users.FindAsync(id);

            return user == null
                ? (ActionResult<Models.Users>)NotFound(
                    new { message = "Usuario no encontrado" }
                    )
                : (ActionResult<Models.Users>)Ok(user);
        }

        // POST: api/users
        [HttpPost]
        public async Task<ActionResult<Models.Users>> CreateUser(Models.Users user)
        {
            user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);

            // Creación del usuario
            _context.Users.Add(user);
            await _context.SaveChangesAsync();


            return Ok(
                new
                {
                    message = "Usuario creado exitosamente",
                    createdUser = new
                    {
                        user.Username,
                        user.Password,
                        user.Email,
                        user.Phone,
                        user.Active
                    }
                }
                );
        }

        // Put: api/users/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, Models.Users user)
        {
            var existingUser = await _context.Users.FindAsync(id);

            if (existingUser == null)
            {
                return NotFound(
                        new
                        {
                            message = "Usuario no encontrado"
                        }
                    );
            }

            existingUser.Username = user.Username;
            existingUser.Password = user.Password;
            existingUser.Email = user.Email;
            existingUser.Phone = user.Phone;
            existingUser.Active = user.Active;

            await _context.SaveChangesAsync();

            return Ok(
                    new
                    {
                        message = "Usuario actualizado correctamente",
                        user = existingUser
                    }
                );
        }

        //Delete: api/users/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DelectingUser(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound(
                        new
                        {
                            message = "Usuario no encontrado"
                        }
                    );
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return Ok(
                new
                {
                    message = "Usuario eliminado exitosamente",
                    user
                }
            );
        }
    }
}
