using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using ServiceDesk.Data.Context;
using ServiceDesk.Data.Model;
using ServiceDesk.Users.Model;

namespace ServiceDesk.Users.Controller;

[Route("api/[controller]")]
[ApiController]
public class UserController(PersistenceContext persistence) : ControllerBase
{
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

        await persistence.Users.AddAsync(user);
        await persistence.SaveChangesAsync();

        return CreatedAtAction(nameof(GetUserDetails), new { id = user.Id }, user);
    }

    [HttpGet("{id:guid}")]
    public async Task<Results<Ok<User>, NotFound<string>>> GetUserDetails([FromRoute] Guid id)
    {
        return await persistence.Users.FindAsync(id) switch
        {
            var user when user != null => TypedResults.Ok(user),
            _ => TypedResults.NotFound($"{nameof(User)} was not found")
        };
    }
}
