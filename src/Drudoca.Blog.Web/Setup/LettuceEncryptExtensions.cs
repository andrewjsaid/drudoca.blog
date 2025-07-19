using LettuceEncrypt;

namespace Drudoca.Blog.Web.Setup;

internal static class LettuceEncryptExtensions
{

    public static void ConfigureLettuceEncrypt(this WebApplicationBuilder builder)
    {
        var lettuceEncryptConfig = builder.Configuration.GetSection("LettuceEncrypt");
        if (!lettuceEncryptConfig.Exists())
        {
            return;
        }

        var lettuceEncryptService = builder.Services.AddLettuceEncrypt();

        var certificateDirectory = lettuceEncryptConfig.GetValue<string?>("CertificateDirectoryPath");
        var certificatePassword = lettuceEncryptConfig.GetValue<string?>("CertificatePassword");
        if (certificateDirectory != null && certificatePassword != null)
        {
            lettuceEncryptService.PersistDataToDirectory(new DirectoryInfo(certificateDirectory), certificatePassword);
        }
    }

}
