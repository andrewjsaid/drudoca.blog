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

        public BlogPostFileConverter(
            ILogger<BlogPostFileConverter> logger,
            StoreOptions storeOptions)
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
            var email = helper.GetOptionalString("email");
            var publishedOn = helper.GetRequiredDate("date");
            var isPublished = helper.GetOptionalBoolean("published") ?? true;
            var isListed = helper.GetOptionalBoolean("listed") ?? true;

            var metaAuthor = helper.GetOptionalString("meta-author");
            var metaDescription = helper.GetOptionalString("meta-description");
            var metaKeywords = helper.GetOptionalString("meta-keywords");

            if (!helper.IsValid)
            {
                return null;
            }

            var metaData = new PageMetaData(
                metaAuthor ?? author,
                metaDescription, 
                metaKeywords);

            var result = new PostData(
                file.Name,
                title, 
                author,
                email,
                publishedOn, 
                isPublished, 
                isListed, 
                file.Markdown, 
                metaData);

            return result;
        }
    }
}
