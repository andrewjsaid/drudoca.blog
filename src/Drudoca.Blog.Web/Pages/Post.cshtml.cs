using System;
using System.Threading.Tasks;
using Drudoca.Blog.Data;
using Drudoca.Blog.DataAccess;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Drudoca.Blog.Web.Pages.Blog
{
    public class PostModel : PageModel
    {
        private readonly IBlogRepository _blogRepository;

        public PostModel(
            IBlogRepository blogRepository)
        {
            _blogRepository = blogRepository;
        }

        public BlogPost? Post { get; private set; }

        public async Task<IActionResult> OnGet(int year, int month, int day, string slug)
        {
            var date = new DateTime(year, month, day);
            var post = await _blogRepository.GetPostAsync(date, slug);
            if (post == null)
            {
                return NotFound();
            }

            Post = post;

            return new PageResult();
        }
    }
}