using ServiceDesk.Data.Enums;

namespace ServiceDesk.Data.Model;

public class Ticket : Entity
{
    public string Subject { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public TicketStatus Status { get; set; }
    public Guid ReporterId { get; set; }
    public Guid? AssigneeId { get; set; }

    public virtual User Reporter { get; set; } = null!;
    public virtual User? Assignee { get; set; }
    public virtual ICollection<Document> Documents { get; set; } = new List<Document>();
}
