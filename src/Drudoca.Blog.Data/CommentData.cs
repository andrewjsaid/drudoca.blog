namespace Drudoca.Blog.Data;

public class CommentData
{
    public required long Id { get; init; }

    public required string PostFileName { get; init; }

    public required long? ParentId { get; init; }

    public required string Author { get; init; }

    public required string Email { get; init; }

    public required string Markdown { get; init; }

    public required DateTime PostedOnUtc { get; init; }

    public required bool IsDeleted { get; init; }
}