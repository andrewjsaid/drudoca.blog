namespace Drudoca.Blog.Domain.Tests;

internal class MockMarkdownParser : IMarkdownParser
{
    public string ToTrustedHtml(string markdown) => markdown;
    public string ToUntrustedHtml(string markdown) => markdown;
}