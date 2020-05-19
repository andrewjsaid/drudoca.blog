using System.Collections.Generic;

namespace Drudoca.Blog.DataAccess.Store
{
    internal class MarkdownFile
    {
        public MarkdownFile(
            string name,
            Dictionary<string, string> headers,
            string markdown)
        {
            Name = name;
            Headers = headers;
            Markdown = markdown;
        }

        public string Name { get; }
        public Dictionary<string,string> Headers { get; }
        public string Markdown { get; }
    }
}
