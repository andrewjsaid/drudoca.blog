using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using Drudoca.Blog.Models;
using HeyRed.MarkdownSharp;
using Microsoft.Extensions.Logging;

namespace Drudoca.Blog.DataAccess
{
    public class BlogPostBuilder : IBlogPostBuilder
    {
        private readonly ILogger<BlogPostBuilder> _logger;

        public BlogPostBuilder(ILogger<BlogPostBuilder> logger)
        {
            _logger = logger;
        }
        
        public BlogPost Build(string fileName, string fileContents)
        {
            var sr = new StringReader(fileContents);
            if (sr.ReadLine() != "---")
            {
                _logger.LogWarning("Blog post must start with ---: {file-name}", fileName);
                throw new InvalidBlogPostFormatException("Blog post must start with ---");
            }

            var blogPost = new BlogPost
            {
                FileName = fileName
            };

            string line;
            while ((line = sr.ReadLine()) != "---")
            {
                ReadAttribute(blogPost, line);
            }

            if (sr.ReadLine() != string.Empty)
            {
                _logger.LogWarning("Blog post must have empty line after ---: {file-name}", fileName);
                throw new InvalidBlogPostFormatException("Blog post must have empty line after header");
            }

            if (string.IsNullOrEmpty(blogPost.Title)
                || string.IsNullOrEmpty(blogPost.Author)
                || blogPost.Date == default(DateTime))
            {
                _logger.LogWarning("Blog post must has incomplete details: {file-name}", fileName);
                throw new InvalidBlogPostFormatException("Missing details from blog post");
            }


            var markdownAppender = new StringBuilder(fileContents.Length);
            while (true)
            {
                line = sr.ReadLine();
                if (string.IsNullOrEmpty(line))
                {
                    line = sr.ReadLine();
                    if (string.IsNullOrEmpty(line))
                    {
                        break;
                    }
                    markdownAppender.AppendLine();
                }
                markdownAppender.AppendLine(line);
            }

            blogPost.Slug = UrlSlug.Slugify(blogPost.Title);
            blogPost.IntroHtml = TransformMarkdown(markdownAppender.ToString());

            markdownAppender.AppendLine();
            markdownAppender.AppendLine();
            while ((line = sr.ReadLine()) != null)
            {
                markdownAppender.AppendLine(line);
            }

            blogPost.FullHtml = TransformMarkdown(markdownAppender.ToString());

            return blogPost;
        }

        private void ReadAttribute(BlogPost blogPost, string line)
        {
            var colonIndex = line.IndexOf(":", StringComparison.OrdinalIgnoreCase);
            if (colonIndex == -1)
            {
                _logger.LogError("Header must contain colon: {line}", line);
                throw new InvalidBlogPostFormatException(line);
            }

            var header = line.Substring(0, colonIndex);
            var value = colonIndex < line.Length - 1 ? line.Substring(colonIndex + 1, line.Length - colonIndex - 1) : string.Empty;

            header = header.Trim().ToLowerInvariant();
            value = value.Trim();

            switch (header)
            {
                case "title":
                    blogPost.Title = value;
                    break;
                case "date":
                    blogPost.Date = DateTime.Parse(value);
                    break;
                case "author":
                    blogPost.Author = value;
                    break;
                case "published":
                    blogPost.IsPublished = bool.Parse(value);
                    break;
            }
        }

        private static string TransformMarkdown(string markdownText)
        {
            var md = new Markdown();
            var result = md.Transform(markdownText);
            return result;
        }
    }
}
