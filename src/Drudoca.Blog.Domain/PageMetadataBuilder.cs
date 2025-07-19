using Drudoca.Blog.Data;

namespace Drudoca.Blog.Domain;

internal class PageMetadataBuilder : IPageMetadataBuilder
{
    public PageMetadata Build(Data.PageMetadata data) 
        => new PageMetadata(data.Author, data.Description, data.Keywords);
}