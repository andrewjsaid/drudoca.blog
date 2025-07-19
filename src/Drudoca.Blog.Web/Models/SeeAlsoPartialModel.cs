using Drudoca.Blog.Domain;

namespace Drudoca.Blog.Web.Models;

public class SeeAlsoPartialModel(BlogPost? mostRecent, BlogPost? previous, BlogPost? next)
{
    public BlogPost? MostRecent { get; } = mostRecent;
    public BlogPost? Previous { get; } = previous;
    public BlogPost? Next { get; } = next;
}