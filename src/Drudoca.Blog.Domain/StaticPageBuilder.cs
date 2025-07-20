using Drudoca.Blog.Data;

namespace Drudoca.Blog.Domain;

internal class StaticPageBuilder(
    IMarkdownParser markdownParser,
    IPageMetadataBuilder pageMetadataBuilder)
    : IStaticPageBuilder
{
    public StaticPage Build(StaticPageData data)
    {
        var html = markdownParser.TrustedToHtml(data.Markdown);

        var pageMetadata = pageMetadataBuilder.Build(data.PageMetadata);

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