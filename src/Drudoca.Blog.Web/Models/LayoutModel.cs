using Drudoca.Blog.Domain;

namespace Drudoca.Blog.Web.Models
{
    public class LayoutModel
    {
        public LayoutModel(
            StaticPageMenuItem[] menu,
            string? googleAnalyticsClientId,
            string? fontAwesomeId)
        {
            Menu = menu;
            GoogleAnalyticsClientId = googleAnalyticsClientId;
            FontAwesomeId = fontAwesomeId;
        }

        public StaticPageMenuItem[] Menu { get; }
        public string? GoogleAnalyticsClientId { get; }
        public string? FontAwesomeId { get;  }
    }
}
