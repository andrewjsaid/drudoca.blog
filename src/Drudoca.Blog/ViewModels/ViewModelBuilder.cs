using System;
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
                Slug = GetSlug(blogPost)
            };
            return result;
        }

        private static string GetSlug(BlogPost blogPost)
        {
            string publishedDateSlugPart = blogPost.PublishedDate.ToString("yyyy-MM");
            string titleSlugPart = UrlSlug.Slugify(blogPost.Title);
            var slug = $"{publishedDateSlugPart}-{titleSlugPart}";
            return slug;
        }

        private string TransformMarkdown(string markdownText)
        {
            var md = new Markdown();
            var result = md.Transform(markdownText);
            return result;
        }
    }
}
