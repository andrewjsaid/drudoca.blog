using Drudoca.Blog.Domain;

namespace Drudoca.Blog.Web.Models
{
    public class PostPartialModel
    {
        public PostPartialModel(
            BlogPost post, 
            bool onlyIntro,
            int? numComments)
        {
            Post = post;
            OnlyIntro = onlyIntro;
            NumComments = numComments;
        }

        public BlogPost Post { get; }
        public bool OnlyIntro { get; }
        public int? NumComments { get; }
    }
}
