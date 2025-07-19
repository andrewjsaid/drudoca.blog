namespace Drudoca.Blog.Domain.Notifications;

public class NotificationEvent(
    string eventName,
    IReadOnlyDictionary<string, string> parameters)
{
    public string EventName { get; } = eventName;

    public IReadOnlyDictionary<string, string> Parameters { get; } = parameters;
}