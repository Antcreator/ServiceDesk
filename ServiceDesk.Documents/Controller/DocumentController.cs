using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using ServiceDesk.Data.Context;
using ServiceDesk.Data.Model;
using ServiceDesk.Documents.Model;

namespace ServiceDesk.Documents.Controller;

[ApiController]
[Route("api/[controller]")]
public class DocumentController(PersistenceContext persistence, IWebHostEnvironment env) : ControllerBase
{
    [HttpGet]
    public IActionResult GetDocumentList()
    {
        var documents = persistence.Documents.ToList();

        return Ok(documents);
    }
    
    [HttpPost]
    public async Task<IActionResult> CreateDocument([FromForm] CreateDocumentDto createDocumentDto)
    {
        if (!ModelState.IsValid)
        {
            return ValidationProblem(ModelState);
        }

        var document = new Document
        {
            TicketId = createDocumentDto.TicketId,
            Title = createDocumentDto.Attachment.FileName,
            File = Path.GetRandomFileName(),
        };

        var file = document.File + Path.GetExtension(createDocumentDto.Attachment.FileName);
        var upload = Path.Combine(env.WebRootPath, "uploads", file);
        using var stream = System.IO.File.Create(upload);

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
}