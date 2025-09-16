using Microsoft.AspNetCore.Mvc;
using SchuifJeAanAPI.Models;

namespace SchuifJeAanAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private static List<User> _users = new List<User>();
        private static int _nextUserId = 1;

        [HttpPost("Create")]
        public ActionResult<int> Create([FromForm] User user)
        {
            if (_users.Any(u => u.Email == user.Email))
            {
                return BadRequest("Email already exists");
            }

            user.UserId = _nextUserId++;
            _users.Add(user);
            return Ok(user.UserId);
        }

        [HttpGet("Get")]
        public ActionResult<User> Get([FromQuery] int id)
        {
            var user = _users.FirstOrDefault(u => u.UserId == id);
            if (user == null)
                return NotFound();

            return Ok(user);
        }

        [HttpGet("SignIn")]
        public ActionResult<User> SignIn([FromQuery] string email, [FromQuery] string password)
        {
            var user = _users.FirstOrDefault(u => u.Email == email && u.Password == password);
            if (user == null)
                return NotFound();

            return Ok(user);
        }

        [HttpPost("Edit")]
        public ActionResult Edit([FromForm] int userId, [FromForm] string bio, [FromForm] string email, [FromForm] string password)
        {
            var user = _users.FirstOrDefault(u => u.UserId == userId);
            if (user == null)
                return NotFound();

            if (!string.IsNullOrEmpty(bio))
                user.Bio = bio;
            if (!string.IsNullOrEmpty(email))
                user.Email = email;
            if (!string.IsNullOrEmpty(password))
                user.Password = password;

            return Ok();
        }

        [HttpDelete("Delete")]
        public ActionResult Delete([FromQuery] int id)
        {
            var user = _users.FirstOrDefault(u => u.UserId == id);
            if (user == null)
                return NotFound();

            _users.Remove(user);
            return Ok();
        }
    }
}
