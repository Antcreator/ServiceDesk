using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ServiceDesk.Data.Context;
using ServiceDesk.Data.Model;
using ServiceDesk.Documents.Model;

namespace ServiceDesk.Documents.Controller;

[ApiController]
[Route("api/[controller]")]
public class DocumentController(PersistenceContext persistence, IWebHostEnvironment env) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetDocumentList()
    {
        var documents = await persistence.Documents.ToListAsync();

        return Ok(documents);
    }
    
    [HttpPost]
    public async Task<IActionResult> CreateDocument([FromForm] CreateDocumentDto createDocumentDto)
    {
        if (!ModelState.IsValid)
        {
            return ValidationProblem(ModelState);
        }

        var file = Path.GetRandomFileName() + Path.GetExtension(createDocumentDto.Attachment.FileName);
        var document = new Document
        {
            TicketId = createDocumentDto.TicketId,
            Title = createDocumentDto.Attachment.FileName,
            File = file,
            DateCreated = DateTime.UtcNow,
            DateModified = DateTime.UtcNow,
        };

        var path = Path.Combine(env.WebRootPath, "uploads", file);
        using var stream = System.IO.File.Create(path);

        await createDocumentDto.Attachment.CopyToAsync(stream);
        await persistence.Documents.AddAsync(document);
        await persistence.SaveChangesAsync();

        return CreatedAtAction(nameof(GetDocumentDetails), new { id = document.Id }, document);
    }

    [HttpGet("{id:guid}")]
    public async Task<Results<Ok<Document>, NotFound<string>>> GetDocumentDetails([FromRoute] Guid id)
    {
        return await persistence.Documents.FindAsync(id) switch
        {
            var document when document != null => TypedResults.Ok(document),
            _ => TypedResults.NotFound($"{nameof(Document)} not found")
        };
    }

    [HttpGet("{id:guid}/download")]
    public async Task<IActionResult> DownloadDocument([FromRoute] Guid id)
    {
        var document = await persistence.Documents.FindAsync(id);

        if (document == null)
        {
            return NotFound($"{nameof(Document)} not found");
        }

        var path = Path.Combine(env.WebRootPath, "uploads", document.File);

        return PhysicalFile(path, "application/octet-stream", document.File);
    }
}