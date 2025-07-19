namespace Drudoca.Blog.Domain;

public class PageMetadata(
    string? author,
    string? description,
    string? keywords)
{
    public string? Author { get; set; } = author;
    public string? Description { get; set; } = description;
    public string? Keywords { get; set; } = keywords;
}