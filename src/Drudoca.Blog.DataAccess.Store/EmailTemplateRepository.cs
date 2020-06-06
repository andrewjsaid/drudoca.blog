using System;
using System.IO;
using System.Threading.Tasks;
using Drudoca.Blog.Data;

namespace Drudoca.Blog.DataAccess.Store
{
    internal class EmailTemplateRepository : IEmailTemplateRepository
    {
        private readonly IMarkdownStore<EmailTemplateData> _store;

        public EmailTemplateRepository(IMarkdownStore<EmailTemplateData> store)
        {
            _store = store;
        }

        public async Task<string?> GetLayoutHtmlAsync()
        {
            await Task.CompletedTask;
            return null;
        }

        public async Task<EmailTemplateData?> GetEmailTemplateAsync(string fileName)
        {
            var files = await _store.GetAllAsync();

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
}
