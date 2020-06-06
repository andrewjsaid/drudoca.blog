using System.Diagnostics;

namespace Drudoca.Blog.Domain
{
    [DebuggerDisplay("{" + nameof(EventName) + "}")]
    internal class ReadyEmail
    {
        public ReadyEmail(
            string eventName,
            string to,
            string? from,
            string[] cc,
            string[] bcc,
            string subject,
            string html)
        {
            EventName = eventName;
            From = from;
            To = to;
            Cc = cc;
            Bcc = bcc;
            Subject = subject;
            Html = html;
        }

        public string EventName { get; }
        public string To { get; }
        public string? From { get; }
        public string[] Cc { get; }
        public string[] Bcc { get; }
        public string Subject { get; }
        public string Html { get; }
    }
}
