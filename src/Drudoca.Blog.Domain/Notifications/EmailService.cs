using System.Net;
using Drudoca.Blog.Data;
using Drudoca.Blog.DataAccess;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MimeKit;
using MimeKit.Text;

namespace Drudoca.Blog.Domain.Notifications;

internal class EmailService(
    IEmailTemplateRepository repository,
    ITemplateEngine templateEngine,
    IMarkdownParser markdownParser,
    IOptions<EmailOptions> emailOptions,
    ILogger<EmailService> logger)
    : IEmailService
{
    public async Task SendEmailAsync(string to, NotificationEvent @event)
    {
        var templateData = await repository.GetEmailTemplateAsync(@event.EventName);

        if (templateData is not { IsEnabled: true })
            return;

        var emailSubject = templateEngine.Execute(templateData.Subject, @event.Parameters);

        string bodyHtml;
        switch (templateData.ContentsType)
        {
            case ContentsType.Markdown:
                var markdown = templateEngine.Execute(templateData.Contents, @event.Parameters);
                bodyHtml = markdownParser.TrustedToHtml(markdown);
                break;

            case ContentsType.Html:
                bodyHtml = templateEngine.Execute(templateData.Contents, @event.Parameters);
                break;

            default: throw new NotSupportedException($"Contents Type {templateData.ContentsType} is not supported for emails.");
        }

        string emailHtml;

        var layoutData = await repository.GetLayoutHtmlAsync();
        if (string.IsNullOrWhiteSpace(layoutData))
        {
            emailHtml = bodyHtml;
        }
        else
        {
            var layoutParameters = new Dictionary<string, string>(@event.Parameters)
            {
                {"body", bodyHtml}
            };

            emailHtml = templateEngine.Execute(layoutData, layoutParameters);
        }

        var result = new ReadyEmail(
            @event.EventName,
            to,
            templateData.From,
            templateData.Cc,
            templateData.Bcc,
            emailSubject,
            emailHtml);

        await SendEmailSmtpAsync(result);
    }

    private async Task SendEmailSmtpAsync(ReadyEmail email)
    {
        var smtp = emailOptions.Value.SmtpServer;
        if (smtp == null || smtp.Accounts.Length == 0)
            return;

        SmtpAccount? account = null;
        if (string.IsNullOrEmpty(email.From))
        {
            account = smtp.Accounts[0];
        }
        else
        {
            foreach (var a in smtp.Accounts)
            {
                if (string.Equals(a.Address, email.From, StringComparison.OrdinalIgnoreCase))
                {
                    account = a;
                }
            }
        }

        if (account == null)
        {
            logger.LogError("No smtp user configured for address '{address}'", email.From);
            return;
        }

        var from = email.From ?? account.Address ?? account.Username;

        try
        {
            using var client = new SmtpClient();

            var sso = Enum.Parse<SecureSocketOptions>(smtp.Mode);
            await client.ConnectAsync(smtp.Host, smtp.Port, sso);

            var credentials = new NetworkCredential(account.Username, account.Password);
            await client.AuthenticateAsync(new SaslMechanismLogin(credentials));
            var message = new MimeMessage();

            message.From.Add(InternetAddress.Parse(from));
            message.To.Add(InternetAddress.Parse(email.To));
            message.Cc.AddRange(Array.ConvertAll(email.Cc, InternetAddress.Parse));
            message.Bcc.AddRange(Array.ConvertAll(email.Bcc, InternetAddress.Parse));

            message.Subject = email.Subject;
            message.Body = new TextPart(TextFormat.Html)
            {
                Text = email.Html
            };

            await client.SendAsync(message);

            await client.DisconnectAsync(true);

        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unable to send email {event} from {from} to {to}", 
                email.EventName, from, email.To);
        }
    }
}