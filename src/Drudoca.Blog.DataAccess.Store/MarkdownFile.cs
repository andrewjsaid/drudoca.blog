namespace Drudoca.Blog.DataAccess.Store;

internal class MarkdownFile(
    string name,
    Dictionary<string, string> headers,
    string markdown)
{
    public string Name { get; } = name;
    public Dictionary<string,string> Headers { get; } = headers;
    public string Markdown { get; } = markdown;
}