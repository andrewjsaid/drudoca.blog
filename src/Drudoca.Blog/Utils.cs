using System;

namespace Drudoca.Blog
{
    internal static class Utils
    {
        /// <summary>
        /// Here to replace the much missed Array.ConvertAll from the full .NET Framework.
        /// </summary>
        public static TOut[] ConvertAll<TIn, TOut>(this TIn[] @this, Func<TIn, TOut> map)
        {
            var result = new TOut[@this.Length];
            for (int i = 0; i < result.Length; i++)
            {
                var @in = @this[i];
                var @out = map(@in);
                result[i] = @out;
            }
            return result;
        }

    }
}
