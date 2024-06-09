using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ServiceDesk.Tickets.Model;

public record CreateTicketDto
{
    [Required]
    public required string Subject { get; set; }
    
    [Required]
    public required string Description { get; set; }
    
    [Required]
    public Guid ReporterId { get; set; }
    
    [Required]
    public required IFormFile Attachment { get; set; }
}
