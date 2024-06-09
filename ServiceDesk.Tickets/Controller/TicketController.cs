using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ServiceDesk.Data.Context;
using ServiceDesk.Data.Model;
using ServiceDesk.Tickets.Model;
using ServiceDesk.Tickets.Service;

namespace ServiceDesk.Tickets.Controller;

[Route("api/[controller]")]
[ApiController]
public class TicketController(PersistenceContext persistence, DocumentService documentService) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> CreateTicket([FromForm] CreateTicketDto createTicketDto)
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
        await documentService.UploadDocument(createTicketDto.Attachment, ticket.Id);

        return CreatedAtAction(nameof(GetTicketDetails), new { id = ticket.Id }, ticket);
    }

    [HttpGet("{id:guid}")]
    public async Task<Results<Ok<Ticket>, NotFound<string>>> GetTicketDetails([FromRoute] Guid id)
    {
        var ticket = await persistence.Tickets
            .Include(e => e.Reporter)
            .Include(e => e.Assignee)
            .Include(e => e.Documents)
            .Where(e => e.Id == id)
            .FirstOrDefaultAsync();

        return ticket == null ? TypedResults.NotFound($"{nameof(Ticket)} not found") : TypedResults.Ok(ticket);
    }
}
