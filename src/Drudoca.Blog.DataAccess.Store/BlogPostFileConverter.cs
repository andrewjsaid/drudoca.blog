using System.Collections.Generic;
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

        public string? DirectoryPath => _storeOptions.BlogPostPath;

        public IComparer<PostData> Comparer { get; } = new MostRecentFirstPostComparer();

        public PostData? Convert(MarkdownFile file)
        {
            var helper = new MarkdownFileHelper(file, _logger);

            var title = helper.GetRequiredString("title");
            var author = helper.GetRequiredString("author");
            var publishedOn = helper.GetRequiredDate("date");
            var isPublished = helper.GetRequiredBoolean("published");
            var isListed = helper.GetRequiredBoolean("listed");

            if (!helper.IsValid)
            {
                return null;
            }

            var result = new PostData(file.Name, title, author, publishedOn, isPublished, isListed, file.Markdown);

            return result;
        }
    }
}
