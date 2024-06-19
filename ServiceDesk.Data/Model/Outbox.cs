namespace ServiceDesk.Data.Model;

public class Outbox : Entity
{
    public Guid EntityId { get; set; } = Guid.Empty;
    public string EntityName { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
}
