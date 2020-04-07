using System.Collections.Generic;
using Drudoca.Blog.Data;

namespace Drudoca.Blog.DataAccess.Store
{
    internal class MostRecentFirstPostComparer : IComparer<BlogPost>
    {
        public int Compare(BlogPost x, BlogPost y) => y.PublishedOn.CompareTo(x.PublishedOn);
    }
}