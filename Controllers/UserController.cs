using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ApiFaceUnah.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly DBContext _context;

        public UserController(DBContext context)
        {
            _context = context;
        }

        // Get: api/users
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Models.UserModel>>> GetUsers()
        {
            var users = await _context.Users.ToListAsync();

            return users == null
                ? (ActionResult<IEnumerable<Models.UserModel>>)NotFound(
                        new { message = "No hay usuarios" }
                    )
                : (ActionResult<IEnumerable<Models.UserModel>>)Ok(
                    new { message = "Usuarios", users }
                );
        }

        // Get: api/users/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Models.UserModel>> GetUsersId(int id)
        {
            var user = await _context.Users.FindAsync(id);

            return user == null
                ? (ActionResult<Models.UserModel>)NotFound(
                    new { message = "Usuario no encontrado" }
                    )
                : (ActionResult<Models.UserModel>)Ok(user);
        }

        // POST: api/users
        [HttpPost]
        public async Task<ActionResult<Models.UserModel>> CreateUser(Models.UserModel user)
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
        public async Task<IActionResult> UpdateUser(int id, Models.UserModel user)
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
        public async Task<IActionResult> DeletingUser(int id)
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
