using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Drudoca.Blog.DataAccess.Store
{
    internal class MarkdownDirectoryReader : IMarkdownDirectoryReader
    {
        private readonly ILogger _logger;

        public MarkdownDirectoryReader(ILogger<MarkdownDirectoryReader> logger)
        {
            _logger = logger;
        }

        public async ValueTask<MarkdownFile[]> ReadAsync(string path)
        {
            try
            {
                _logger.LogDebug("Loading files from {path}", path);
                
                var directoryInfo = new DirectoryInfo(path);
                var fileInfos = directoryInfo.GetFiles("*.md");

                _logger.LogInformation("Found {count} *.md files in {path}", fileInfos.Length, path);

                var result = new List<MarkdownFile>(fileInfos.Length);
                foreach (var fileInfo in fileInfos)
                {
                    var mdFile = await CreateMarkdownFileAsync(fileInfo);
                    if (mdFile != null)
                    {
                        result.Add(mdFile);
                    }
                }

                return result.ToArray();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading markdown files in {path}", path);
                return Array.Empty<MarkdownFile>();
            }
        }

        private async Task<MarkdownFile?> CreateMarkdownFileAsync(FileInfo fileInfo)
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

                var result = new MarkdownFile(fileInfo.Name, headers, markdown);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Could not create blog post from {file}", fileInfo.FullName);
                return null;
            }
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
