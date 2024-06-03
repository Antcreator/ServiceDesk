using System.Text.Json.Serialization;

namespace ServiceDesk.Data.Model;

public class Document : Entity
{
    public string Title { get; set; } = string.Empty;
    public string File { get; set; } = string.Empty;
    public Guid TicketId { get; set; }

    [JsonIgnore]
    public virtual Ticket Ticket { get; set; } = null!;
}
