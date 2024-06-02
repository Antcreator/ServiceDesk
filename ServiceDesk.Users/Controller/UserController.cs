using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using ServiceDesk.Data.Context;
using ServiceDesk.Data.Model;
using ServiceDesk.Users.Model;

namespace ServiceDesk.Users.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly PersistenceContext _persistence;

        public UserController(PersistenceContext persistence)
        {
            _persistence = persistence;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateUserDto createUserDto)
        {
            var user = new User
            {
                FirstName = createUserDto.FirstName,
                LastName = createUserDto.LastName,
                Email = createUserDto.Email,
                Password = createUserDto.Password,
                Role = createUserDto.Role,
            };

            await _persistence.Users.AddAsync(user);
            await _persistence.SaveChangesAsync();

            return CreatedAtAction(nameof(GetUserDetails), new { id = user.Id }, user);
        }

        [HttpGet("{id:guid}")]
        public async Task<Results<Ok<User>, NotFound<string>>> GetUserDetails([FromRoute] Guid id)
        {
            return await _persistence.Users.FindAsync(id) switch
            {
                var user when user != null => TypedResults.Ok(user),
                _ => TypedResults.NotFound($"{nameof(User)} was not found")
            };
        }
    }
}
