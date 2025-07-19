using Drudoca.Blog.Data;

namespace Drudoca.Blog.DataAccess;

public interface IEmailTemplateRepository
{
    Task<string?> GetLayoutHtmlAsync();

    Task<EmailTemplateData?> GetEmailTemplateAsync(string fileName);
}