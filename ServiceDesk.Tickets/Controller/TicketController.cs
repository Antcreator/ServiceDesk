using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using ServiceDesk.Data.Context;
using ServiceDesk.Data.Model;
using ServiceDesk.Tickets.Model;

namespace ServiceDesk.Tickets.Controller;

[Route("api/[controller]")]
[ApiController]
public class TicketController(PersistenceContext persistence) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> CreateTicket([FromBody] CreateTicketDto createTicketDto)
    {
        if (!ModelState.IsValid)
        {
            return ValidationProblem(ModelState);
        }
        
        var ticket = new Ticket
        {
            Subject = createTicketDto.Subject,
            Description = createTicketDto.Description,
            ReporterId = createTicketDto.ReporterId,
        };

        await persistence.Tickets.AddAsync(ticket);
        await persistence.SaveChangesAsync();

        return CreatedAtAction(nameof(GetTicketDetails), new { id = ticket.Id }, ticket);
    }

    [HttpGet("{id:guid}")]
    public async Task<Results<Ok<Ticket>, NotFound<string>>> GetTicketDetails([FromRoute] Guid id)
    {
        return await persistence.Tickets.FindAsync(id) switch
        {
            var ticket when ticket != null => TypedResults.Ok(ticket),
            _ => TypedResults.NotFound($"{nameof(Ticket)} not found")
        };
    }
}
