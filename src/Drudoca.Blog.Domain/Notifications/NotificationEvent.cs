using System.Collections.Generic;

namespace Drudoca.Blog.Domain.Notifications
{
    public class NotificationEvent
    {
        public NotificationEvent(
            string eventName,
            IReadOnlyDictionary<string, string> parameters)
        {
            EventName = eventName;
            Parameters = parameters;
        }

        public string EventName { get; }
        public IReadOnlyDictionary<string, string> Parameters { get; }
    }
}
