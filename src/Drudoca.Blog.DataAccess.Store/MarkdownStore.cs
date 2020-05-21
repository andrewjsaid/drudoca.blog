using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Drudoca.Blog.DataAccess.Store
{
    internal class MarkdownStore<T> : IMarkdownStore<T> where T : class
    {
        private readonly IMarkdownDirectoryReader _reader;
        private readonly IMarkdownFileConverter<T> _converter;
        private readonly ILogger _logger;

        public MarkdownStore(
            IMarkdownDirectoryReader reader,
            IMarkdownFileConverter<T> converter,
            ILogger<MarkdownStore<T>> logger)
        {
            _reader = reader;
            _converter = converter;
            _logger = logger;
        }

        public virtual async ValueTask<T[]> GetAllAsync()
        {
            try
            {
                var path = _converter.DirectoryPath;
                if (path == null)
                    return Array.Empty<T>();

                var files = await _reader.ReadAsync(path);

                var results = new List<T>(files.Length);
                foreach (var file in files)
                {
                    var result = _converter.Convert(file);
                    if (result != null)
                    {
                        results.Add(result);
                    }
                }

                if (_converter.Comparer != null)
                {
                    results.Sort(_converter.Comparer);
                }

                return results.ToArray();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Could not load store ({type})", typeof(T).Name);
                return Array.Empty<T>();
            }
        }

    }
}