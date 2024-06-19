﻿using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Net.Http.Headers;
using ServiceDesk.Data.Context;
using ServiceDesk.Data.Model;
using ServiceDesk.Tickets.Model;
using ServiceDesk.Tickets.Service;
using ServiceDesk.Util;

namespace ServiceDesk.Tickets.Controller;

[Route("api/[controller]")]
[ApiController]
public class TicketController(PersistenceContext persistence, DocumentService documentService, QueueService queue) : ControllerBase
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
            Id = Guid.NewGuid(),
            Subject = createTicketDto.Subject,
            Description = createTicketDto.Description,
            ReporterId = createTicketDto.ReporterId,
            DateCreated = DateTime.UtcNow,
            DateModified = DateTime.UtcNow,
        };
        var outbox = new Outbox
        {
            EntityId = ticket.Id,
            EntityName = typeof(Ticket).Name,
            Message = $"{nameof(Ticket)} added",
            DateCreated = DateTime.UtcNow,
            DateModified = DateTime.UtcNow,
        };

        await persistence.Tickets.AddAsync(ticket);
        await persistence.Outboxes.AddAsync(outbox);
        await persistence.SaveChangesAsync();
        // await documentService.UploadDocument(createTicketDto.Attachment, ticket.Id);

        Response.Headers.Append(HeaderNames.Baggage, $"outbox={outbox.Id}");

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
