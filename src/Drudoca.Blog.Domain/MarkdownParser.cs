using Markdig;
using Markdig.Parsers.Inlines;
using Markdig.Renderers.Html;
using Markdig.Renderers;
using Markdig.Syntax.Inlines;
using System.Reflection.Metadata;
using Markdig.Renderers.Html.Inlines;

namespace Drudoca.Blog.Domain;

internal class MarkdownParser : IMarkdownParser
{
    private readonly Lazy<MarkdownPipeline> _trustedPipeline = new(CreateTrustedPipeline);
    private readonly Lazy<MarkdownPipeline> _untrustedPipeline = new(CreateUntrustedPipeline);

    private static MarkdownPipeline CreateTrustedPipeline()
    {
        var pb = new MarkdownPipelineBuilder();
        pb.Extensions.Add(new OnlyPublishedLinksMarkdownExtension());
        return pb.Build();
    }

    private static MarkdownPipeline CreateUntrustedPipeline()
    {
        var pb = new MarkdownPipelineBuilder();
        pb.DisableHtml();
        pb.DisableHeadings();
        pb.InlineParsers.TryRemove<LinkInlineParser>();

        return pb.Build();
    }

    public string TrustedToHtml(string markdown) => RenderToHtml(markdown, _trustedPipeline.Value);

    public string UntrustedToHtml(string markdown) => RenderToHtml(markdown, _untrustedPipeline.Value);

    private static string RenderToHtml(string markdown, MarkdownPipeline pipeline)
    {
        var document = Markdown.Parse(markdown, pipeline);

        var htmlRenderer = new HtmlRenderer(new StringWriter());

        htmlRenderer.ObjectRenderers.RemoveAll(r => r is CodeInlineRenderer);
        htmlRenderer.ObjectRenderers.Add(new CSharpInlineCodeRenderer());

        htmlRenderer.Render(document);

        var result = htmlRenderer.Writer.ToString();
        return result ?? string.Empty;
    }

    public class CSharpInlineCodeRenderer : HtmlObjectRenderer<CodeInline>
    {
        protected override void Write(HtmlRenderer renderer, CodeInline obj)
        {
            renderer.Write("<code class=\"language-csharp\">");
            renderer.WriteEscape(obj.Content);
            renderer.Write("</code>");
        }
    }
}