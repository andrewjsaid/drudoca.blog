using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Drudoca.Blog.Config;
using Drudoca.Blog.Data;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Drudoca.Blog.DataAccess.Store
{
    internal class BlogStore : IBlogStore
    {

        private readonly ILogger _logger;
        private readonly IOptionsMonitor<BlogOptions> _siteOptions;

        public BlogStore(
            ILogger<BlogStore> logger,
            IOptionsMonitor<BlogOptions> siteOptions)
        {
            _logger = logger;
            _siteOptions = siteOptions;
        }

        public virtual async ValueTask<BlogPostData[]> GetAllAsync()
        {
            try
            {
                var configPath = _siteOptions.CurrentValue.BlogFolder;
                var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", configPath);

                _logger.LogDebug("Loading files from {path}", path);

                var directoryInfo = new DirectoryInfo(path);
                var fileInfos = directoryInfo.GetFiles("*.md");

                _logger.LogInformation("Found {count} *.md files in blog-posts folder", fileInfos.Length);

                var result = new List<BlogPostData>(fileInfos.Length);
                foreach (var fileInfo in fileInfos)
                {
                    var blogPost = await CreateBlogPostAsync(fileInfo);
                    if (blogPost != null)
                    {
                        result.Add(blogPost);
                    }
                }

                result.Sort(new MostRecentFirstPostComparer());
                return result.ToArray();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Could not load any blog posts");
                return Array.Empty<BlogPostData>();
            }
        }

        private async Task<BlogPostData?> CreateBlogPostAsync(FileInfo fileInfo)
        {
            try
            {
                var headers = new Dictionary<string, string>();
                string markdown;

                using (var streamReader = new StreamReader(fileInfo.OpenRead()))
                {
                    var line = await streamReader.ReadLineAsync();
                    if (line != "---")
                    {
                        _logger.LogWarning("File {file-name} is missing the header", fileInfo.Name);
                        return null;
                    }

                    while ((line = await streamReader.ReadLineAsync()) != "---")
                    {
                        if (line == null)
                        {
                            _logger.LogWarning("Unable to parse header for file {file-name}", fileInfo.Name);
                            return null;
                        }

                        var (key, value) = ParseHeaderLine(fileInfo.Name, line);
                        if (key != null)
                        {
                            headers.Add(key, value);
                        }
                    }

                    markdown = await streamReader.ReadToEndAsync();
                }

                var result = CreatePost(headers, markdown);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Could not create blog post from {FILE}", fileInfo.FullName);
                return null;
            }
        }

        private BlogPostData? CreatePost(Dictionary<string, string> headers, string markdown)
        {
            bool valid = true;

            if (!headers.TryGetValue("title", out var title))
            {
                _logger.LogWarning("File {file-name} has missing title");
                title = string.Empty;
                valid = false;
            }

            if (!headers.TryGetValue("author", out var author))
            {
                _logger.LogWarning("File {file-name} has missing author");
                author = string.Empty;
                valid = false;
            }

            if (!headers.TryGetValue("date", out var dateString))
            {
                _logger.LogWarning("File {file-name} has missing date");
                dateString = string.Empty;
                valid = false;
            }

            if (!DateTime.TryParse(dateString, out var publishedOn))
            {
                _logger.LogWarning("File {file-name} has invalid date");
                valid = false;
            }

            if (!headers.TryGetValue("published", out var publishedString))
            {
                _logger.LogWarning("File {file-name} has missing published");
                publishedString = string.Empty;
                valid = false;
            }

            if (!bool.TryParse(publishedString, out var isPublished))
            {
                _logger.LogWarning("File {file-name} has invalid published");
                valid = false;
            }

            if (!valid)
            {
                return null;
            }

            var result = new BlogPostData(title, author, publishedOn, isPublished, markdown);
            return result;
        }

        private (string? key, string value) ParseHeaderLine(string fileName, string line)
        {
            if (line == string.Empty)
            {
                return (null, string.Empty);
            }

            var index = line.IndexOf(':');
            if (index == -1)
            {
                _logger.LogWarning("Could not parse header line in {file-name}: {line}", fileName, line);
                return (null, string.Empty);
            }

            var key = line.Substring(0, index).Trim();
            var value = index == line.Length - 1 ? string.Empty : line.Substring(index + 1).Trim();
            return (key, value);
        }

    }
}