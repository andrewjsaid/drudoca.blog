using System;
using Markdig;
using Markdig.Parsers.Inlines;

namespace Drudoca.Blog.Domain
{
    internal class MarkdownParser : IMarkdownParser
    {
        private readonly Lazy<MarkdownPipeline> _trustedPipeline;
        private readonly Lazy<MarkdownPipeline> _untrustedPipeline;

        public MarkdownParser()
        {
            _trustedPipeline = new Lazy<MarkdownPipeline>(CreatePostPipeline);
            _untrustedPipeline = new Lazy<MarkdownPipeline>(CreateCommentPipeline);
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

        public string ToTrustedHtml(string markdown)
        {
            var html = Markdown.ToHtml(markdown, _trustedPipeline.Value);
            return html;
        }

        public string ToUntrustedHtml(string markdown)
        {
            var html = Markdown.ToHtml(markdown, _untrustedPipeline.Value);
            return html;
        }
    }
}
