using Drudoca.Blog.Data;

namespace Drudoca.Blog.Domain
{
    internal class PostBuilder : IPostBuilder
    {
        private readonly IMarkdownParser _markdownParser;

        public PostBuilder(IMarkdownParser markdownParser)
        {
            _markdownParser = markdownParser;
        }

        public BlogPost Build(PostData data)
        {
            var title = UrlSlug.Slugify(data.Title);
            var html = _markdownParser.ToPostHtml(data.Markdown);

            string? introHtml = null;

            var mainSectionIndex = data.Markdown.IndexOf("\n[//]: # (Main Section)");
            if (mainSectionIndex > -1)
            {
                var introMarkdown = data.Markdown.Substring(0, mainSectionIndex);
                introHtml = _markdownParser.ToPostHtml(introMarkdown);
            }

            var result = new BlogPost(
                data.FileName,
                data.Title,
                data.Author,
                data.PublishedOn,
                data.IsPublished,
                data.IsListed,
                title,
                html,
                introHtml
            );
            return result;
        }
    }
}
