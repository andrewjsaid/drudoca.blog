using System.Diagnostics;

namespace Drudoca.Blog.Data
{
    [DebuggerDisplay("{" + nameof(FileName) + "}")]
    public class EmailTemplateData
    {

        public EmailTemplateData(
            string fileName,
            string? from,
            string[] cc,
            string[] bcc,
            string subject,
            bool isEnabled,
            ContentsType contentsType,
            string contents)
        {
            FileName = fileName;
            From = from;
            Cc = cc;
            Bcc = bcc;
            Subject = subject;
            IsEnabled = isEnabled;
            ContentsType = contentsType;
            Contents = contents;
        }

        public string FileName { get; }
        public string? From { get; }
        public string[] Cc { get; }
        public string[] Bcc { get; }
        public string Subject { get; }
        public bool IsEnabled { get; }

        public ContentsType ContentsType { get; }
        public string Contents { get; }
    }
}
