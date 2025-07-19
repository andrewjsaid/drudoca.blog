using Drudoca.Blog.Data;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Drudoca.Blog.DataAccess.Store;

internal class StaticPageFileConverter(
    ILogger<StaticPageFileConverter> logger,
    IOptions<StoreOptions> storeOptions)
    : IMarkdownFileConverter<StaticPageData>
{
    private readonly ILogger _logger = logger;

    public string? DirectoryPath => storeOptions.Value.StaticPagePath;

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

        var metaData = new PageMetadata
        {
            Author = metaAuthor,
            Description = metaDescription,
            Keywords = metaKeywords
        };

        var result = new StaticPageData
        {
            FileName = file.Name,
            Title = title,
            UriSegment = uriSegment,
            IsPublished = isPublished,
            MenuSequence = menuSequence,
            MenuIcon = menuIcon,
            MenuText = menuText,
            Markdown = file.Markdown,
            PageMetadata = metaData
        };

        return result;
    }

    private class SequenceStaticPageComparer : IComparer<StaticPageData>
    {
        public int Compare(StaticPageData? x, StaticPageData? y)
            => (x?.MenuSequence ?? default).CompareTo(y?.MenuSequence ?? default);
    }
}