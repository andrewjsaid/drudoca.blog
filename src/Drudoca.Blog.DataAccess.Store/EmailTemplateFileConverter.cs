using System.Collections.Generic;
using Drudoca.Blog.Config;
using Drudoca.Blog.Data;
using Microsoft.Extensions.Logging;

namespace Drudoca.Blog.DataAccess.Store
{
    internal class EmailTemplateFileConverter : IMarkdownFileConverter<EmailTemplateData>
    {
        private readonly ILogger _logger;
        private readonly StoreOptions _storeOptions;

        public EmailTemplateFileConverter(ILogger<EmailTemplateFileConverter> logger, StoreOptions storeOptions)
        {
            _logger = logger;
            _storeOptions = storeOptions;
        }

        public string? DirectoryPath => _storeOptions.EmailTemplatePath;

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

            var result = new EmailTemplateData(
                file.Name,
                from,
                cc,
                bcc,
                subject,
                isEnabled,
                ContentsType.Markdown,
                file.Markdown);

            return result;
        }
    }
}
