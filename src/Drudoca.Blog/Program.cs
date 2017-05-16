using System.IO;
using Microsoft.AspNetCore.Hosting;

namespace Drudoca.Blog
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = new WebHostBuilder()
                .UseKestrel()
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseStartup<Startup>()
                .UseApplicationInsights()
                .UseUrls("http://localhost:5000");

            var host = builder.Build();
            host.Run();
        }
    }
}
