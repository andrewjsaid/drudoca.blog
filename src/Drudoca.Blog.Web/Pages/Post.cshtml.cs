using System;
using System.Threading.Tasks;
using Drudoca.Blog.Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Drudoca.Blog.Web.Pages
{
    public class PostModel : PageModel
    {
        private readonly IBlogManager _blogManager;

        public PostModel(
            IBlogManager blogManager)
        {
            _blogManager = blogManager;
        }

        public BlogPost? Post { get; private set; }

        public async Task<IActionResult> OnGet(int year, int month, int day, string slug)
        {
            var date = new DateTime(year, month, day);
            var post = await _blogManager.GetPostAsync(date, slug);
            if (post == null)
            {
                return NotFound();
            }

            Post = post;

            return new PageResult();
        }
    }
}