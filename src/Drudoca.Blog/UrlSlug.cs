using System.Text.RegularExpressions;

namespace Drudoca.Blog
{
    // Based on http://stackoverflow.com/questions/2920744/url-slugify-algorithm-in-c

    internal static class UrlSlug
    {
        public static string Slugify(string value)
        {
            //First to lower case
            value = value.ToLowerInvariant();

            //Replace spaces
            value = Regex.Replace(value, @"\s", "-", RegexOptions.Compiled);

            //Remove invalid chars
            value = Regex.Replace(value, @"[^a-z0-9\s-_]", "", RegexOptions.Compiled);

            //Trim dashes from end
            value = value.Trim('-', '_');

            //Replace double occurences of - or _
            value = Regex.Replace(value, @"([-_]){2,}", "$1", RegexOptions.Compiled);

            return value;
        }
    }
}
