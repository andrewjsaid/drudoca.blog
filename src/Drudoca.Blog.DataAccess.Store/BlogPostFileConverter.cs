using Drudoca.Blog.Data;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Drudoca.Blog.DataAccess.Store;

internal class BlogPostFileConverter(
    ILogger<BlogPostFileConverter> logger,
    IOptions<StoreOptions> storeOptions)
    : IMarkdownFileConverter<PostData>
{
    private readonly ILogger _logger = logger;

    public string? DirectoryPath => storeOptions.Value.BlogPostPath;

    public IComparer<PostData> Comparer { get; } = new MostRecentFirstPostComparer();

    public PostData? Convert(MarkdownFile file)
    {
        var helper = new MarkdownFileHelper(file, _logger);

        var title = helper.GetRequiredString("title");
        var author = helper.GetRequiredString("author");
        var email = helper.GetOptionalString("email");
        var publishedOn = helper.GetRequiredDate("date");
        var isPublished = helper.GetOptionalBoolean("published") ?? true;
        var isListed = helper.GetOptionalBoolean("listed") ?? true;

        var metaAuthor = helper.GetOptionalString("meta-author");
        var metaDescription = helper.GetOptionalString("meta-description");
        var metaKeywords = helper.GetOptionalString("meta-keywords");

        if (!helper.IsValid)
        {
            return null;
        }

        var metaData = new PageMetadata
        {
            Author = metaAuthor ?? author,
            Description = metaDescription,
            Keywords = metaKeywords
        };

        var result = new PostData
        {
            FileName = file.Name,
            Title = title,
            Author = author,
            Email = email,
            PublishedOn = publishedOn,
            IsPublished = isPublished,
            IsListed = isListed,
            Markdown = file.Markdown,
            PageMetadata = metaData
        };

        return result;
    }
    private class MostRecentFirstPostComparer : IComparer<PostData>
    {
        public int Compare(PostData? x, PostData? y)
            => (y?.PublishedOn ?? default).CompareTo(x?.PublishedOn ?? default);
    }
}