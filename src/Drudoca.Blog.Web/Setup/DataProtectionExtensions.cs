using Microsoft.AspNetCore.DataProtection;

namespace Drudoca.Blog.Web.Setup;

internal static class DataProtectionExtensions
{

    public static void ConfigureDataProtection(this WebApplicationBuilder builder)
    {
        var dataProtectionPath = builder.Configuration.GetSection("Site:DataProtectionPath").Value;
        if (string.IsNullOrEmpty(dataProtectionPath))
        {
            return;
        }

        builder.Services
            .AddDataProtection()
            .PersistKeysToFileSystem(new DirectoryInfo(dataProtectionPath));
    }

}