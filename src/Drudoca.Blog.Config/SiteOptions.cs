using System;

namespace Drudoca.Blog.Config
{
    public class SiteOptions
    {
        public string[] Administrators { get; set; } = Array.Empty<string>();
        public string? DataProtectionPath { get; set; }
        public string? GoogleTagManagerClientId { get; set; }
        public string? FontAwesomeId { get; set; }
    }
}
