namespace Drudoca.Blog.Domain.Tests;

internal class MockMarkdownParser : IMarkdownParser
{
    public string TrustedToHtml(string markdown) => markdown;
    public string UntrustedToHtml(string markdown) => markdown;
}