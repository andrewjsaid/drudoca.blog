namespace Drudoca.Blog.Data
{
    public class PageMetaData
    {
        public PageMetaData(
            string? author,
            string? description,
            string? keywords)
        {
            Author = author;
            Description = description;
            Keywords = keywords;
        }

        public string? Author { get; }
        public string? Description { get; }
        public string? Keywords { get; }
    }
}
