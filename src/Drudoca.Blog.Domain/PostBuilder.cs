using Drudoca.Blog.Data;

namespace Drudoca.Blog.Domain;

internal class PostBuilder(
    IMarkdownParser markdownParser,
    IPageMetadataBuilder pageMetadataBuilder)
    : IPostBuilder
{
    public BlogPost Build(PostData data)
    {
        var title = UrlSlug.Slugify(data.Title);
        var html = markdownParser.TrustedToHtml(data.Markdown);

        string? introHtml = null;

        var mainSectionIndex = data.Markdown.IndexOf("\n[//]: # (Main Section)", StringComparison.Ordinal);
        if (mainSectionIndex > -1)
        {
            var introMarkdown = data.Markdown[..mainSectionIndex];
            introHtml = markdownParser.TrustedToHtml(introMarkdown);
        }

        var pageMetadata = pageMetadataBuilder.Build(data.PageMetadata);

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