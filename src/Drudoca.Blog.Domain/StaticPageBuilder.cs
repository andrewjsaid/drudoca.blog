using Drudoca.Blog.Data;

namespace Drudoca.Blog.Domain
{
    internal class StaticPageBuilder: IStaticPageBuilder
    {
        private readonly IMarkdownParser _markdownParser;
        private readonly IPageMetadataBuilder _pageMetadataBuilder;

        public StaticPageBuilder(
            IMarkdownParser markdownParser,
            IPageMetadataBuilder pageMetadataBuilder)
        {
            _markdownParser = markdownParser;
            _pageMetadataBuilder = pageMetadataBuilder;
        }

        public StaticPage Build(StaticPageData data)
        {
            var html = _markdownParser.ToTrustedHtml(data.Markdown);

            var pageMetadata = _pageMetadataBuilder.Build(data.PageMetaData);

            var result = new StaticPage(
                data.FileName,
                data.Title,
                data.UriSegment,
                data.IsPublished,
                html,
                pageMetadata);

            return result;
        }
    }
}
