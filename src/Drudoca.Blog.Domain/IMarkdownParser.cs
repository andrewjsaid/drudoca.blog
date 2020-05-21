namespace Drudoca.Blog.Domain
{
    internal interface IMarkdownParser
    {
        string ToStaticPageHtml(string markdown);
        string ToPostHtml(string markdown);
        string ToCommentHtml(string markdown);
    }
}
