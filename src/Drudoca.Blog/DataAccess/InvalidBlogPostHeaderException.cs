using System;

namespace Drudoca.Blog.DataAccess
{
    public class InvalidBlogPostFormatException : Exception
    {
        public InvalidBlogPostFormatException()
        {
        }

        public InvalidBlogPostFormatException(string message) : base(message)
        {
        }
    }
}