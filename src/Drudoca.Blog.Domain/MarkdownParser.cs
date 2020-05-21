using System;
using Markdig;
using Markdig.Parsers.Inlines;

namespace Drudoca.Blog.Domain
{
    internal class MarkdownParser : IMarkdownParser
    {
        private readonly Lazy<MarkdownPipeline> _postPipeline;
        private readonly Lazy<MarkdownPipeline> _commentPipeline;

        public MarkdownParser()
        {
            _postPipeline = new Lazy<MarkdownPipeline>(CreatePostPipeline);
            _commentPipeline = new Lazy<MarkdownPipeline>(CreateCommentPipeline);
        }

        private static MarkdownPipeline CreatePostPipeline()
        {
            return new MarkdownPipelineBuilder().Build();
        }

        private static MarkdownPipeline CreateCommentPipeline()
        {
            var pb = new MarkdownPipelineBuilder();
            pb.DisableHtml();
            pb.DisableHeadings();
            pb.InlineParsers.TryRemove<LinkInlineParser>();

            return pb.Build();
        }

        public string ToStaticPageHtml(string markdown)
        {
            var html = Markdown.ToHtml(markdown, _postPipeline.Value);
            return html;
        }

        public string ToPostHtml(string markdown)
        {
            var html = Markdown.ToHtml(markdown, _postPipeline.Value);
            return html;
        }

        public string ToCommentHtml(string markdown)
        {
            var html = Markdown.ToHtml(markdown, _commentPipeline.Value);
            return html;
        }
    }
}
