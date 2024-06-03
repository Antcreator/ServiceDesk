using ServiceDesk.Data.Model.Enums;

namespace ServiceDesk.Data.Model;

public class User : Entity
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public required UserRole Role { get; set; }

    public ICollection<Ticket> Issues = new List<Ticket>();
    public ICollection<Ticket> Tasks = new List<Ticket>();
}
