using Drudoca.Blog.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Drudoca.Blog.DataAccess
{
    internal class BlogPostSource : IBlogPostSource
    {
        private const int StartedLogEvent = 1;
        private const int FoundFilesLogEvent = 2;
        private const int CouldNotParseFileNameLogEvent = 3;
        private const int CouldNotLoadSingleLogEvent = 4;
        private const int CouldNotLoadAnyLogEvent = 5;

        private readonly ILogger _logger;

        public BlogPostSource(ILogger<BlogPostSource> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<BlogPost[]> LoadAsync()
        {
            try
            {
                var directoryPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "blog-posts");

                _logger.LogDebug(StartedLogEvent, "Loading files from {PATH}", directoryPath);

                var directoryInfo = new DirectoryInfo(directoryPath);
                var fileInfos = directoryInfo.GetFiles("*.md");

                _logger.LogInformation(FoundFilesLogEvent, "Found {COUNT} *.md files in blog-posts folder", fileInfos.Length);

                var result = new List<BlogPost>(fileInfos.Length);
                foreach (var fileInfo in fileInfos)
                {
                    var blogPost = await CreateBlogPostAsync(fileInfo);
                    if (blogPost != null)
                    {
                        result.Add(blogPost);
                    }
                }
                return result.ToArray();
            }
            catch (Exception ex)
            {
                _logger.LogError(CouldNotLoadAnyLogEvent, ex, "Could not load any blog posts");
                return new BlogPost[0];
            }
        }

        private static readonly Regex _fileNameRegex = new Regex(@"^(?<Y>\d{4})(?<M>\d{2})(?<D>\d{2})_(?<Title>.+)\.md$");
        private async Task<BlogPost> CreateBlogPostAsync(FileInfo fileInfo)
        {
            try
            {
                var fileName = fileInfo.Name;

                var fileNameMatch = _fileNameRegex.Match(fileName);
                if (!fileNameMatch.Success)
                {
                    _logger.LogWarning(CouldNotParseFileNameLogEvent, "File name does not match expected format yyyyMMdd_Title.md: {FILENAME}", fileName);
                    return null;
                }

                var title = fileNameMatch.Groups["Title"].Value;
                var yyyy = int.Parse(fileNameMatch.Groups["Y"].Value);
                var mm = int.Parse(fileNameMatch.Groups["M"].Value);
                var dd = int.Parse(fileNameMatch.Groups["D"].Value);

                string fileContents;
                using(var streamReader = new StreamReader(fileInfo.OpenRead()))
                {
                    fileContents = await streamReader.ReadToEndAsync();
                }

                var result = new BlogPost
                {
                    Title = title,
                    PublishedDate = new DateTime(yyyy, mm, dd),
                    Markdown = fileContents
                };
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(CouldNotLoadSingleLogEvent, ex, "Could not create blog post from {FILE}", fileInfo.FullName);
                return null;
            }
        }
    }
}