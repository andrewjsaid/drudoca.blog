using Drudoca.Blog.Data;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Drudoca.Blog.DataAccess.Store;

internal class EmailTemplateFileConverter(
    ILogger<EmailTemplateFileConverter> logger,
    IOptions<StoreOptions> storeOptions)
    : IMarkdownFileConverter<EmailTemplateData>
{
    private readonly ILogger _logger = logger;

    public string? DirectoryPath => storeOptions.Value.EmailTemplatePath;

    public IComparer<EmailTemplateData> Comparer { get; } = new EmailTemplateComparer();

    public EmailTemplateData? Convert(MarkdownFile file)
    {
        var helper = new MarkdownFileHelper(file, _logger);

        var subject = helper.GetRequiredString("subject");
        var from = helper.GetOptionalString("from");
        var isEnabled = helper.GetOptionalBoolean("enabled") ?? true;
        var cc = helper.GetOptionalStringList("cc");
        var bcc = helper.GetOptionalStringList("bcc");

        if (!helper.IsValid)
        {
            return null;
        }

        var result = new EmailTemplateData
        {
            FileName = file.Name,
            From = from,
            Cc = cc,
            Bcc = bcc,
            Subject = subject,
            IsEnabled = isEnabled,
            ContentsType = ContentsType.Markdown,
            Contents = file.Markdown
        };

        return result;
    }

    private class EmailTemplateComparer : IComparer<EmailTemplateData>
    {
        public int Compare(EmailTemplateData? x, EmailTemplateData? y)
            => string.CompareOrdinal(x?.FileName, y?.FileName);
    }
}