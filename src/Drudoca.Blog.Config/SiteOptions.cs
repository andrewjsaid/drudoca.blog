using System;

namespace Drudoca.Blog.Config
{
    public class SiteOptions
    {
        public string[] Administrators { get; set; } = Array.Empty<string>();
        public string? DataProtectionPath { get; set; }
    }
}
