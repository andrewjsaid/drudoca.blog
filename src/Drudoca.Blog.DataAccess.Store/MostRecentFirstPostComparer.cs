using System;
using System.Collections.Generic;
using Drudoca.Blog.Data;

namespace Drudoca.Blog.DataAccess.Store
{
    internal class MostRecentFirstPostComparer : IComparer<PostData>
    {
        public int Compare(PostData? x, PostData? y)
            => (y?.PublishedOn ?? default) .CompareTo(x?.PublishedOn ?? default);
    }
}