using System.ComponentModel.DataAnnotations;

namespace Drudoca.Blog.Web.Models
{
    public class PostPageCommentForm
    {
        public long? ParentId { get; set; }

        [Required]
        [Display(Name = "Comment")]
        public string Markdown { get; set; } = default!;
    }
}
