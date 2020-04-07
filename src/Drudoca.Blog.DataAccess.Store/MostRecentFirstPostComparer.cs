using System.Collections.Generic;
using Drudoca.Blog.Data;

namespace Drudoca.Blog.DataAccess.Store
{
    internal class MostRecentFirstPostComparer : IComparer<BlogPostData>
    {
        public int Compare(BlogPostData x, BlogPostData y) => y.PublishedOn.CompareTo(x.PublishedOn);
    }
}