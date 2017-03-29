using Drudoca.Blog.Models;
using HeyRed.MarkdownSharp;

namespace Drudoca.Blog.ViewModels
{
    public class ViewModelBuilder : IViewModelBuilder
    {
        public BlogPostViewModel Build(BlogPost blogPost)
        {
            var result = new BlogPostViewModel
            {
                Title = blogPost.Title,
                PublishedDate = blogPost.PublishedDate.Date,
                RawHtml = TransformMarkdown(blogPost.Markdown),
                Slug = blogPost.Slug
            };
            return result;
        }
        
        private string TransformMarkdown(string markdownText)
        {
            var md = new Markdown();
            var result = md.Transform(markdownText);
            return result;
        }
    }
}
