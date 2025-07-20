namespace Drudoca.Blog.Domain;

internal interface IMarkdownParser
{
    string TrustedToHtml(string markdown);
    string UntrustedToHtml(string markdown);
}