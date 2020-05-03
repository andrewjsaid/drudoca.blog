using System.Diagnostics;
using System.Threading.Tasks;
using Drudoca.Blog.Domain;
using Drudoca.Blog.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Drudoca.Blog.Web.Pages
{
    [ValidateAntiForgeryToken]
    public class PostModel : PageModel
    {
        private readonly IBlogService _blogManager;

        public PostModel(
            IBlogService blogManager)
        {
            _blogManager = blogManager;
        }

        [BindProperty(SupportsGet = true), FromRoute]
        public PostUrlModel PostUrl { get; set; } = default!;

        [BindProperty]
        public PostPageCommentForm? CommentForm { get; set; }

        public BlogPost? Post { get; private set; }
        public BlogComment[]? Comments { get; private set; }

        public async Task<IActionResult> OnGet()
        {
            Post = await _blogManager.GetPostAsync(PostUrl.GetDate(), PostUrl.Slug);
            if (Post == null)
            {
                return NotFound();
            }

            Comments = await _blogManager.GetCommentsAsync(Post.FileName);

            return Page();
        }

        public async Task<IActionResult> OnPost()
        {
            Post = await _blogManager.GetPostAsync(PostUrl.GetDate(), PostUrl.Slug);
            if (Post == null)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var form = CommentForm;
                Debug.Assert(form != null, "Model can not be valid with null form");
                var id = await _blogManager.CreateCommentAsync(
                    Post.FileName,
                    form.ParentId,
                    form.Author,
                    form.Email,
                    form.Markdown);

                if (id != null)
                {
                    CommentForm = null;

                    return RedirectToPage(nameof(Post), null, null, $"c{id}");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Unable to save comment.");
                }
            }

            // If we got to here, the input is wrong.
            Comments = await _blogManager.GetCommentsAsync(Post.FileName);
            return Page();
        }
    }
}