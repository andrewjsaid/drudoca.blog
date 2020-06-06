using System;
using Drudoca.Blog.Data;

namespace Drudoca.Blog.Domain
{
    internal class PostBuilder : IPostBuilder
    {
        private readonly IMarkdownParser _markdownParser;
        private readonly IPageMetadataBuilder _pageMetadataBuilder;

        public PostBuilder(
            IMarkdownParser markdownParser,
            IPageMetadataBuilder pageMetadataBuilder)
        {
            _markdownParser = markdownParser;
            _pageMetadataBuilder = pageMetadataBuilder;
        }

        public BlogPost Build(PostData data)
        {
            var title = UrlSlug.Slugify(data.Title);
            var html = _markdownParser.ToTrustedHtml(data.Markdown);

            string? introHtml = null;

            var mainSectionIndex = data.Markdown.IndexOf("\n[//]: # (Main Section)", StringComparison.Ordinal);
            if (mainSectionIndex > -1)
            {
                var introMarkdown = data.Markdown.Substring(0, mainSectionIndex);
                introHtml = _markdownParser.ToTrustedHtml(introMarkdown);
            }

            var pageMetadata = _pageMetadataBuilder.Build(data.PageMetaData);

            var result = new BlogPost(
                data.FileName,
                data.Title,
                data.Author,
                data.PublishedOn,
                data.IsPublished,
                data.IsListed,
                title,
                html,
                introHtml,
                pageMetadata
            );
            return result;
        }
    }
}
