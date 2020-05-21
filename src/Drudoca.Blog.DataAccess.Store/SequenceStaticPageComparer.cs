using System.Collections.Generic;
using Drudoca.Blog.Data;

namespace Drudoca.Blog.DataAccess.Store
{
    internal class SequenceStaticPageComparer : IComparer<StaticPageData>
    {
        public int Compare(StaticPageData x, StaticPageData y)
            => x.MenuSequence.GetValueOrDefault().CompareTo(y.MenuSequence.GetValueOrDefault());
    }
}