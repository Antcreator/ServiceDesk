namespace ServiceDesk.Data.Model;

public class Document : Entity
{
    public string Title { get; set; } = string.Empty;
    public string File { get; set; } = string.Empty;
    public Guid TicketId { get; set; }

    public virtual Ticket Ticket { get; set; } = null!;
}
