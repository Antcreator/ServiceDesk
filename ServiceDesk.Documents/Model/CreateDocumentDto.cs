using System.ComponentModel.DataAnnotations;

namespace ServiceDesk.Documents.Model;

public record CreateDocumentDto
{
    [Required]
    public required Guid TicketId { get; set; }
    
    [Required]
    public required IFormFile Attachment { get; set; }
}