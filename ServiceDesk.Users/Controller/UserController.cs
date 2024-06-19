using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Net.Http.Headers;
using ServiceDesk.Data.Context;
using ServiceDesk.Data.Model;
using ServiceDesk.Users.Model;
using ServiceDesk.Util.Service;

namespace ServiceDesk.Users.Controller;

[Route("api/[controller]")]
[ApiController]
public class UserController(PersistenceContext persistence, PasswordHasherService hasherService) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetUserList()
    {
        var users = await persistence.Users.ToListAsync();

        return Ok(users);
    }
    
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateUserDto createUserDto)
    {
        var hash = hasherService.GetHash(createUserDto.Password);
        var user = new User
        {
            Id = Guid.NewGuid(),
            FirstName = createUserDto.FirstName,
            LastName = createUserDto.LastName,
            Email = createUserDto.Email,
            Password = hash,
            Role = createUserDto.Role,
            DateCreated = DateTime.UtcNow,
            DateModified = DateTime.UtcNow,
        };
        var outbox = new Outbox
        {
            EntityId = user.Id,
            EntityName = typeof(User).Name,
            Message = $"{nameof(User)} added",
            DateCreated = DateTime.UtcNow,
            DateModified = DateTime.UtcNow,
        };

        await persistence.Users.AddAsync(user);
        await persistence.Outboxes.AddAsync(outbox);
        await persistence.SaveChangesAsync();

        Response.Headers.Append(HeaderNames.Baggage, $"outbox={outbox.Id}");

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
