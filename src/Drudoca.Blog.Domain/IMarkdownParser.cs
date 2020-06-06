namespace Drudoca.Blog.Domain
{
    internal interface IMarkdownParser
    {
        string ToTrustedHtml(string markdown);
        string ToUntrustedHtml(string markdown);
    }
}
