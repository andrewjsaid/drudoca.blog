using System.Collections.Generic;

namespace Drudoca.Blog.Domain.Notifications
{
    internal interface ITemplateEngine
    {
        string Execute(string template, IReadOnlyDictionary<string, string> parameters);
    }
}