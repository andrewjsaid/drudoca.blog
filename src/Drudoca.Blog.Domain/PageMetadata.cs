namespace Drudoca.Blog.Domain
{
    public class PageMetadata
    {
        public PageMetadata(
            string? author,
            string? description,
            string? keywords)
        {
            Author = author;
            Description = description;
            Keywords = keywords;
        }

        public string? Author { get; set; }
        public string? Description { get; set; }
        public string? Keywords { get; set; }
    }
}
