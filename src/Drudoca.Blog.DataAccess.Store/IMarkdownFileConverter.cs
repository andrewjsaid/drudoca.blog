namespace Drudoca.Blog.DataAccess.Store;

internal interface IMarkdownFileConverter<T> where T : class
{
    string? DirectoryPath { get; }
    IComparer<T> Comparer { get; }

    T? Convert(MarkdownFile file);
}