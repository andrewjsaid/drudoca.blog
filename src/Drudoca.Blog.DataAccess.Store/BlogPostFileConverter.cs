using System;
using System.Collections.Generic;
using System.IO;
using Drudoca.Blog.Config;
using Drudoca.Blog.Data;
using Microsoft.Extensions.Logging;

namespace Drudoca.Blog.DataAccess.Store
{
    internal class BlogPostFileConverter : IMarkdownFileConverter<PostData>
    {
        private readonly ILogger _logger;
        private readonly StoreOptions _storeOptions;

        public BlogPostFileConverter(ILogger<BlogPostFileConverter> logger, StoreOptions storeOptions)
        {
            _logger = logger;
            _storeOptions = storeOptions;
        }

        public string DirectoryPath
        {
            get
            {
                var configPath = _storeOptions.BlogPostPath;
                var path = Path.Combine(Directory.GetCurrentDirectory(), configPath);
                return path;
            }
        }

        public IComparer<PostData> Comparer { get; } = new MostRecentFirstPostComparer();

        public PostData? Convert(MarkdownFile file)
        {
            var headers = file.Headers;
            var fileName = file.Name;

            var valid = true;

            if (!headers.TryGetValue("title", out var title))
            {
                _logger.LogWarning("File {file-name} has missing title", fileName);
                title = string.Empty;
                valid = false;
            }

            if (!headers.TryGetValue("author", out var author))
            {
                _logger.LogWarning("File {file-name} has missing author", fileName);
                author = string.Empty;
                valid = false;
            }

            if (!headers.TryGetValue("date", out var dateString))
            {
                _logger.LogWarning("File {file-name} has missing date", fileName);
                dateString = string.Empty;
                valid = false;
            }

            if (!DateTime.TryParse(dateString, out var publishedOn))
            {
                _logger.LogWarning("File {file-name} has invalid date", fileName);
                valid = false;
            }

            if (!headers.TryGetValue("published", out var publishedString))
            {
                _logger.LogWarning("File {file-name} has missing published", fileName);
                publishedString = string.Empty;
                valid = false;
            }

            if (!bool.TryParse(publishedString, out var isPublished))
            {
                _logger.LogWarning("File {file-name} has invalid published", fileName);
                valid = false;
            }

            if (!headers.TryGetValue("listed", out var listedString))
            {
                _logger.LogWarning("File {file-name} has missing listed", fileName);
                listedString = string.Empty;
                valid = false;
            }

            if (!bool.TryParse(listedString, out var isListed))
            {
                _logger.LogWarning("File {file-name} has invalid published", fileName);
                valid = false;
            }

            if (!valid)
            {
                return null;
            }

            var result = new PostData(fileName, title, author, publishedOn, isPublished, isListed, file.Markdown);
            return result;
        }
    }
}
