﻿namespace Drudoca.Blog.Web;

public class SeoOptions
{
    public string? MetaDescription { get; set; }
    public string? MetaKeywords { get; set; }
    public string? Robots { get; set; } = "index, follow";
}