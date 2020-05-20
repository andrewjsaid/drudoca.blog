using System.Collections.Generic;
using System.IO;
using Drudoca.Blog.Config;
using Drudoca.Blog.Data;
using Microsoft.Extensions.Logging;

namespace Drudoca.Blog.DataAccess.Store
{
    internal class StaticPageFileConverter : IMarkdownFileConverter<StaticPageData>
    {
        private readonly ILogger _logger;
        private readonly StoreOptions _storeOptions;

        public StaticPageFileConverter(ILogger<StaticPageFileConverter> logger, StoreOptions storeOptions)
        {
            _logger = logger;
            _storeOptions = storeOptions;
        }

        public string DirectoryPath
        {
            get
            {
                var configPath = _storeOptions.StaticPagePath;
                var path = Path.Combine(Directory.GetCurrentDirectory(), configPath);
                return path;
            }
        }

        public IComparer<StaticPageData> Comparer { get; } = new SequenceStaticPageComparer();

        public StaticPageData? Convert(MarkdownFile file)
        {
            var helper = new MarkdownFileHelper(file, _logger);

            var uriSegment = helper.GetRequiredString("uri-segment");
            var menuIcon = helper.GetRequiredString("menu-icon");
            var menuText = helper.GetRequiredString("menu-text");
            var isPublished = helper.GetRequiredBoolean("published");
            var sequence = helper.GetRequiredInt32("sequence");

            if (!helper.IsValid)
            {
                return null;
            }

            var result = new StaticPageData(
                file.Name,
                uriSegment,
                menuIcon,
                menuText,
                isPublished,
                sequence,
                file.Markdown);

            return result;
        }
    }
}
