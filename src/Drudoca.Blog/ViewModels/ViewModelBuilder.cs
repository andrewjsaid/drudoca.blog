using Drudoca.Blog.Models;

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
                RawHtml = blogPost.Markdown,
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
    }
}
