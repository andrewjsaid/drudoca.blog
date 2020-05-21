namespace Drudoca.Blog.Domain.Tests
{
    internal class MockMarkdownParser : IMarkdownParser
    {
        public string ToStaticPageHtml(string markdown) => markdown;
        public string ToPostHtml(string markdown) => markdown;
        public string ToCommentHtml(string markdown) => markdown;
    }
}
