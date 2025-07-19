namespace Drudoca.Blog.Domain.Notifications;

internal class EmailOptions
{
    public SmtpServer? SmtpServer { get; set; }
}

internal class SmtpServer
{
    public string Host { get; set; } = default!;
    public int Port { get; set; } = 465;
    public string Mode { get; set; } = "SslOnConnect";

    public SmtpAccount[] Accounts { get; set; } = [];
}

internal class SmtpAccount
{
    public string Address { get; set; } = default!;
    public string Username { get; set; } = default!;
    public string Password { get; set; } = default!;
}