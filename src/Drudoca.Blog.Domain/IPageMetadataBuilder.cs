using Drudoca.Blog.Data;

namespace Drudoca.Blog.Domain
{
    internal interface IPageMetadataBuilder
    {
        PageMetadata Build(PageMetaData data);
    }
}
