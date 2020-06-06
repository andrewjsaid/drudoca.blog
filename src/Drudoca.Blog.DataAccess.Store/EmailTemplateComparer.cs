using System.Collections.Generic;
using Drudoca.Blog.Data;

namespace Drudoca.Blog.DataAccess.Store
{
    internal class EmailTemplateComparer : IComparer<EmailTemplateData>
    {
        public int Compare(EmailTemplateData x, EmailTemplateData y)
            => string.CompareOrdinal(x.FileName, y.FileName);
    }
}
