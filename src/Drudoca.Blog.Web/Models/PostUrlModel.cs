using System;

namespace Drudoca.Blog.Web.Models
{
    public class PostUrlModel
    {
        public int Year { get; set; }
        public int Month { get; set; }
        public int Day { get; set; }
        public string Slug { get; set; } = default!;

        public DateTime GetDate() => new DateTime(Year, Month, Day);

        public override string ToString() => $"/{Year}/{Month}/{Day}/{Slug}";
    }
}
