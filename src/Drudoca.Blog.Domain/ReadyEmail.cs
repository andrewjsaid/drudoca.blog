using System.Diagnostics;

namespace Drudoca.Blog.Domain;

[DebuggerDisplay("{" + nameof(EventName) + "}")]
internal class ReadyEmail(
    string eventName,
    string to,
    string? from,
    string[] cc,
    string[] bcc,
    string subject,
    string html)
{
    public string EventName { get; } = eventName;
    public string To { get; } = to;
    public string? From { get; } = from;
    public string[] Cc { get; } = cc;
    public string[] Bcc { get; } = bcc;
    public string Subject { get; } = subject;
    public string Html { get; } = html;
}