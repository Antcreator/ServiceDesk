namespace ServiceDesk.Notifications;

public interface INotification
{
    Task Notify(string message);
}
