using Drudoca.Blog.Data;

namespace Drudoca.Blog.Domain
{
    internal class PageMetadataBuilder : IPageMetadataBuilder
    {
        public PageMetadata Build(PageMetaData data) 
            => new PageMetadata(data.Author, data.Description, data.Keywords);
    }
}
