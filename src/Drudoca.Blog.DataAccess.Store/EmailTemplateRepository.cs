using Drudoca.Blog.Data;

namespace Drudoca.Blog.DataAccess.Store;

internal class EmailTemplateRepository(IMarkdownStore<EmailTemplateData> store) : IEmailTemplateRepository
{
    public async Task<string?> GetLayoutHtmlAsync()
    {
        await Task.CompletedTask;
        return null;
    }

    public async Task<EmailTemplateData?> GetEmailTemplateAsync(string fileName)
    {
        var files = await store.GetAllAsync();

        foreach (var file in files)
        {
            var fileNameNoExt = Path.GetFileNameWithoutExtension(file.FileName);
            if (string.Equals(fileNameNoExt, fileName, StringComparison.OrdinalIgnoreCase))
            {
                return file;
            }
        }

        return null;
    }
}