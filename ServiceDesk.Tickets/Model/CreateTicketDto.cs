using System.ComponentModel.DataAnnotations;

namespace ServiceDesk.Tickets.Model;

public record CreateTicketDto
{
    [Required]
    public required string Subject { get; set; }
    
    [Required]
    public required string Description { get; set; }
    
    [Required]
    public Guid ReporterId { get; set; }
    
    public IFormFile? Attachment { get; set; }
}
