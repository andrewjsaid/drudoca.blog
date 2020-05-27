using System;
using System.Collections.Generic;

namespace Drudoca.Blog.Config
{
    public class SiteOptions
    {
        public string[] Administrators { get; set; } = Array.Empty<string>();
        public string? DataProtectionPath { get; set; }
        public string? GoogleTagManagerClientId { get; set; }
        public string? FontAwesomeId { get; set; }
        public string? ThemeColor { get; set; }

        public Dictionary<string, string> MetaTags { get; set; } = new Dictionary<string, string>();
    }
}
