namespace ServiceDesk.Data.Enums;

public enum TicketStatus : byte
{
    PENDING = 0,
    ASSIGNED = 1,
    COMPLETED = 2,
    CLOSED = 3,
    REJECTED = 4,
}
