using System.Threading.Tasks;

namespace Drudoca.Blog.Domain.Notifications
{
    public interface IEmailService
    {
        Task SendEmailAsync(string to, NotificationEvent @event);
    }
}
