using Drudoca.Blog.Domain;

namespace Drudoca.Blog.Web.Models
{
    public class SeeAlsoPartialModel
    {
        public SeeAlsoPartialModel(BlogPost? mostRecent, BlogPost? previous, BlogPost? next)
        {
            MostRecent = mostRecent;
            Previous = previous;
            Next = next;
        }

        public BlogPost? MostRecent { get; }
        public BlogPost? Previous { get; }
        public BlogPost? Next { get; }
    }
}
