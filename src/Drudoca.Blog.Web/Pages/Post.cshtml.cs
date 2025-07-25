﻿using System.Diagnostics;
using System.Security.Claims;
using Drudoca.Blog.Domain;
using Drudoca.Blog.Web.Filters;
using Drudoca.Blog.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Drudoca.Blog.Web.Pages;

[ValidateAntiForgeryToken]
[LayoutModel]
public class PostModel(IBlogService blogService) : PageModel
{
    [BindProperty(SupportsGet = true), FromRoute]
    public PostUrlModel PostUrl { get; set; } = default!;

    public string AuthorName => User.Identity?.Name ?? string.Empty;
    public string AuthorEmail => User.FindFirstValue(ClaimTypes.Email)!;

    [BindProperty]
    public PostPageCommentForm? CommentForm { get; set; }

    public BlogPost? Post { get; private set; }
    public SeeAlsoPartialModel? SeeAlso { get; private set; }
    public BlogComment[]? Comments { get; private set; }
        
    public async Task<IActionResult> OnGet()
    {
        Post = await blogService.GetPostAsync(PostUrl.GetDate(), PostUrl.Slug);
        if (Post == null)
        {
            return NotFound();
        }

        Comments = await blogService.GetCommentsAsync(Post.FileName);
        await BuildSeeAlsoAsync(Post);
        return Page();
    }

    public async Task<IActionResult> OnPost()
    {
        // Can't use authorization filters on single methods
        if (User.Identity?.IsAuthenticated != true)
            return Challenge();

        Post = await blogService.GetPostAsync(PostUrl.GetDate(), PostUrl.Slug);
        if (Post == null)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            var form = CommentForm;
            Debug.Assert(form != null, "Model can not be valid with null form");
            var id = await blogService.CreateCommentAsync(
                Post.FileName,
                form.ParentId,
                AuthorName,
                AuthorEmail,
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
        Comments = await blogService.GetCommentsAsync(Post.FileName);
        await BuildSeeAlsoAsync(Post);
        return Page();
    }

    private async Task BuildSeeAlsoAsync(BlogPost post)
    {
        var mostRecent = await blogService.GetMostRecentPostAsync();

        if (mostRecent?.FileName == post.FileName)
            mostRecent = null;

        var (earlier, later) = await blogService.GetSurroundingPostsAsync(post.FileName);

        SeeAlso = new SeeAlsoPartialModel(mostRecent, earlier, later);
    }
}