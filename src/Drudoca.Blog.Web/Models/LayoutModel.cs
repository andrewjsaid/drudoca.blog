using System.Collections.Generic;
using Drudoca.Blog.Config;
using Drudoca.Blog.Domain;

namespace Drudoca.Blog.Web.Models
{
    public class LayoutModel
    {
        public LayoutModel(
            StaticPageMenuItem[] menu,
            SeoOptions seo,
            string? googleAnalyticsClientId,
            string? fontAwesomeId, 
            string? themeColor, 
            IReadOnlyDictionary<string, string> metaTags)
        {
            Menu = menu;
            Seo = seo;
            GoogleAnalyticsClientId = googleAnalyticsClientId;
            FontAwesomeId = fontAwesomeId;
            ThemeColor = themeColor;
            MetaTags = metaTags;
        }

        public StaticPageMenuItem[] Menu { get; }
        public SeoOptions Seo { get; set; }
        public string? GoogleAnalyticsClientId { get; }
        public string? FontAwesomeId { get; }
        public string? ThemeColor { get; set; }
        public IReadOnlyDictionary<string, string> MetaTags { get; }
    }
}
