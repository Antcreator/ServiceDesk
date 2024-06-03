using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ServiceDesk.Tickets.Model;

public record CreateTicketDto
{
    [Required]
    [JsonPropertyName("subject")]
    public required string Subject { get; set; }
    
    [Required]
    [JsonPropertyName("description")]
    public required string Description { get; set; }
    
    [Required]
    [JsonPropertyName("reporterId")]
    public Guid ReporterId { get; set; }
}
