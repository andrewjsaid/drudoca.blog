using Drudoca.Blog.Data;

namespace Drudoca.Blog.Domain
{
    internal class StaticPageBuilder: IStaticPageBuilder
    {
        private readonly IMarkdownParser _markdownParser;

        public StaticPageBuilder(IMarkdownParser markdownParser)
        {
            _markdownParser = markdownParser;
        }

        public StaticPage Build(StaticPageData data)
        {
            var html = _markdownParser.ToStaticPageHtml(data.Markdown);

            var result = new StaticPage(
                data.FileName,
                data.Title,
                data.UriSegment,
                data.IsPublished,
                html);

            return result;
        }
    }
}
