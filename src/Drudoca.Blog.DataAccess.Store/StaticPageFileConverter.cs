using System.Collections.Generic;
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

        public string? DirectoryPath => _storeOptions.StaticPagePath;

        public IComparer<StaticPageData> Comparer { get; } = new SequenceStaticPageComparer();

        public StaticPageData? Convert(MarkdownFile file)
        {
            var helper = new MarkdownFileHelper(file, _logger);

            var title = helper.GetRequiredString("title");
            var uriSegment = helper.GetRequiredString("uri-segment");
            var isPublished = helper.GetOptionalBoolean("published") ?? true;
            var menuIcon = helper.GetOptionalString("menu-icon");
            var menuText = helper.GetOptionalString("menu-text");
            var menuSequence = helper.GetOptionalInt32("menu-sequence");

            var metaAuthor = helper.GetOptionalString("meta-author");
            var metaDescription = helper.GetOptionalString("meta-description");
            var metaKeywords = helper.GetOptionalString("meta-keywords");

            if (!helper.IsValid)
            {
                return null;
            }

            var metaData = new PageMetaData(
                metaAuthor,
                metaDescription,
                metaKeywords);

            var result = new StaticPageData(
                file.Name,
                title,
                uriSegment,
                isPublished,
                menuSequence,
                menuIcon,
                menuText,
                file.Markdown,
                metaData);

            return result;
        }
    }
}
