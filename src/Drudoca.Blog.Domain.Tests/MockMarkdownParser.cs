namespace Drudoca.Blog.Domain.Tests
{
    internal class MockMarkdownParser : IMarkdownParser
    {
        public string ToPostHtml(string markdown) => markdown;
        public string ToCommentHtml(string markdown) => markdown;
    }
}
