using System.Text.RegularExpressions;
using Markdig;
using Markdig.Renderers;
using Markdig.Renderers.Html.Inlines;
using Markdig.Syntax.Inlines;

namespace Drudoca.Blog.Domain;

internal class OnlyPublishedLinksMarkdownExtension : IMarkdownExtension
{
    public void Setup(MarkdownPipelineBuilder pipeline)
    {
        // Reuse the link
    }

    public void Setup(MarkdownPipeline pipeline, IMarkdownRenderer renderer)
    {
        if (renderer is HtmlRenderer htmlRenderer)
        {
            htmlRenderer.ObjectRenderers.Replace<LinkInlineRenderer>(new OnlyPublishedLinksInlineRenderer());
        }
    }

    internal class OnlyPublishedLinksInlineRenderer : LinkInlineRenderer
    {
        private static readonly Regex _regex = new (@"^/(?<y>\d{4})/(?<m>\d{1,2})/(?<d>\d{1,2})/", RegexOptions.Compiled);

        private static bool IsLinkToUnpublished(LinkInline link)
        {
            var match = _regex.Match(link.Url ?? string.Empty);
            if (!match.Success)
                return false; // Do not reject

            var y = int.Parse(match.Groups["y"].Value);
            var m = int.Parse(match.Groups["m"].Value);
            var d = int.Parse(match.Groups["d"].Value);

            var datePublished = new DateTime(y, m, d);
            var today = DateTime.UtcNow.Date;

            // We could check the exact time and go that way, but I think it's
            // fine to only enable links by the next day. Nobody will notice
            // and it simplifies the logic here.

            return datePublished >= today;
        }

        protected override void Write(HtmlRenderer renderer, LinkInline link)
        {
            if (IsLinkToUnpublished(link))
            {
                renderer.WriteChildren(link);
            }
            else
            {
                base.Write(renderer, link);
            }
        }
    }
}