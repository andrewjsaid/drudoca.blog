using Drudoca.Blog.Data;
using Markdig;

namespace Drudoca.Blog.Domain
{
    internal class PostBuilder : IPostBuilder
    {

        public BlogPost Build(PostData data)
        {
            var title = UrlSlug.Slugify(data.Title);
            var html = Markdown.ToHtml(data.Markdown);

            string? introHtml = null;

            var mainSectionIndex = data.Markdown.IndexOf("\n[//]: # (Main Section)");
            if (mainSectionIndex > -1)
            {
                var introMarkdown = data.Markdown.Substring(0, mainSectionIndex);
                introHtml = Markdown.ToHtml(introMarkdown);
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
