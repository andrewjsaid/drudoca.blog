using Microsoft.Extensions.Logging;

namespace Drudoca.Blog.DataAccess.Store;

internal class MarkdownStore<T>(
    IMarkdownDirectoryReader reader,
    IMarkdownFileConverter<T> converter,
    ILogger<MarkdownStore<T>> logger)
    : IMarkdownStore<T>
    where T : class
{
    private readonly ILogger _logger = logger;

    public virtual async ValueTask<T[]> GetAllAsync()
    {
        try
        {
            var path = converter.DirectoryPath;
            if (path == null)
                return [];

            var files = await reader.ReadAsync(path);

            var results = new List<T>(files.Length);
            foreach (var file in files)
            {
                var result = converter.Convert(file);
                if (result != null)
                {
                    results.Add(result);
                }
            }

            results.Sort(converter.Comparer);

            return results.ToArray();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Could not load store ({type})", typeof(T).Name);
            return [];
        }
    }

}