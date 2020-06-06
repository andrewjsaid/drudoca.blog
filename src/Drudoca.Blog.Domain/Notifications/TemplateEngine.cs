using System.Collections.Generic;
using System.Text;

namespace Drudoca.Blog.Domain.Notifications
{
    internal class TemplateEngine : ITemplateEngine
    {
        public string Execute(string template, IReadOnlyDictionary<string, string> parameters)
        {
            var result = new StringBuilder(template);
            foreach (var (key, value) in parameters)
            {
                result = result.Replace("{" + key + "}", value);
            }
            return result.ToString();
        }
    }
}
