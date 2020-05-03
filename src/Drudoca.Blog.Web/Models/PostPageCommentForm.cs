using System.ComponentModel.DataAnnotations;

namespace Drudoca.Blog.Web.Models
{
    public class PostPageCommentForm
    {
        public long? ParentId { get; set; }

        [Required]
        public string Author { get; set; } = default!;

        [Required]
        public string Email { get; set; } = default!;

        [Required]
        [Display(Name = "Comment")]
        public string Markdown { get; set; } = default!;
    }
}
