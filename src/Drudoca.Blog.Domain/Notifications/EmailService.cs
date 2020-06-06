using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Drudoca.Blog.Config;
using Drudoca.Blog.Data;
using Drudoca.Blog.DataAccess;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Logging;
using MimeKit;
using MimeKit.Text;

namespace Drudoca.Blog.Domain.Notifications
{
    internal class EmailService : IEmailService
    {
        private readonly IEmailTemplateRepository _repository;
        private readonly ITemplateEngine _templateEngine;
        private readonly IMarkdownParser _markdownParser;
        private readonly EmailOptions _emailOptions;
        private readonly ILogger<EmailService> _logger;

        public EmailService(
            IEmailTemplateRepository repository,
            ITemplateEngine templateEngine,
            IMarkdownParser markdownParser,
            EmailOptions emailOptions,
            ILogger<EmailService> logger)
        {
            _repository = repository;
            _markdownParser = markdownParser;
            _emailOptions = emailOptions;
            _logger = logger;
            _templateEngine = templateEngine;
        }

        public async Task SendEmailAsync(string to, NotificationEvent @event)
        {
            var templateData = await _repository.GetEmailTemplateAsync(@event.EventName);

            if (templateData == null || !templateData.IsEnabled)
                return;

            var emailSubject = _templateEngine.Execute(templateData.Subject, @event.Parameters);

            string bodyHtml;
            switch (templateData.ContentsType)
            {
                case ContentsType.Markdown:
                    var markdown = _templateEngine.Execute(templateData.Contents, @event.Parameters);
                    bodyHtml = _markdownParser.ToTrustedHtml(markdown);
                    break;

                case ContentsType.Html:
                    bodyHtml = _templateEngine.Execute(templateData.Contents, @event.Parameters);
                    break;

                default: throw new NotSupportedException($"Contents Type {templateData.ContentsType} is not supported for emails.");
            }

            string emailHtml;

            var layoutData = await _repository.GetLayoutHtmlAsync();
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

                emailHtml = _templateEngine.Execute(layoutData, layoutParameters);
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
            var smtp = _emailOptions.SmtpServer;
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
                _logger.LogError("No smtp user configured for address '{address}'", email.From);
                return;
            }

            var from = email.From ?? account.Address ?? account.Username;

            try
            {
                using var client = new SmtpClient();

                var sso = Enum.Parse<SecureSocketOptions>(smtp.Mode);
                await client.ConnectAsync(smtp.Host, smtp.Port, sso);

                var creds = new NetworkCredential(account.Username, account.Password);
                await client.AuthenticateAsync(new SaslMechanismLogin(creds));
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
                _logger.LogError(ex, "Unable to send email {event} from {from} to {to}", 
                    email.EventName, from, email.To);
            }
        }
    }
}