using Drudoca.Blog.Domain;

namespace Drudoca.Blog.Web.Models;

public class PostPartialModel(
    BlogPost post,
    bool onlyIntro,
    int? numComments)
{
    public BlogPost Post { get; } = post;
    public bool OnlyIntro { get; } = onlyIntro;
    public int? NumComments { get; } = numComments;
}