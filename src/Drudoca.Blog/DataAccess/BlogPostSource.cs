using Drudoca.Blog.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Drudoca.Blog.DataAccess
{
    internal class BlogPostSource : IBlogPostSource
    {

        private const int CouldNotLoadSingleBlogPostLogEvent = 1;
        private const int CouldNotLoadAnyBlogPostLogEvent = 2;

        private readonly ILogger _logger;
        private readonly IBlogPostBuilder _builder;

        public BlogPostSource(ILogger<BlogPostSource> logger, IBlogPostBuilder builder)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _builder = builder ?? throw new ArgumentNullException(nameof(builder)); 
        }

        public async Task<BlogPost[]> LoadAsync()
        {
            try
            {
                var directoryPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "blog-posts");

                _logger.LogDebug("Loading files from {path}", directoryPath);

                var directoryInfo = new DirectoryInfo(directoryPath);
                var fileInfos = directoryInfo.GetFiles("*.md");

                _logger.LogInformation("Found {count} *.md files in blog-posts folder", fileInfos.Length);

                var result = new List<BlogPost>(fileInfos.Length);
                foreach (var fileInfo in fileInfos)
                {
                    string fileContents;
                    using (var streamReader = new StreamReader(fileInfo.OpenRead()))
                    {
                        fileContents = await streamReader.ReadToEndAsync();
                    }

                    try
                    {
                        var blogPost = _builder.Build(fileInfo.Name, fileContents);
                        if (blogPost.IsPublished)
                        {
                            result.Add(blogPost);
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(CouldNotLoadSingleBlogPostLogEvent, ex, "Could not create blog post from {FILE}", fileInfo.Name);
                    }
                }

                result.Sort(BlogPost.MostRecentFirstComparer);

                return result.ToArray();
            }
            catch (Exception ex)
            {
                _logger.LogCritical(CouldNotLoadAnyBlogPostLogEvent, ex, "Could not load any blog posts");
                return new BlogPost[0];
            }
        }

    }
}