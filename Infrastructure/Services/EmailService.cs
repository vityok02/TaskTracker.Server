using Application.Abstract.Interfaces;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;

namespace Infrastructure.Services;

public class EmailService : IEmailService
{
    public async Task SendEmailAsync(
        string email,
        string subject,
        string message,
        CancellationToken cancellationToken = default)
    {
        var emailMessage = new MimeMessage();

        emailMessage.From.Add(new MailboxAddress("Task tracker app", "reenbittasktracker@gmail.com"));
        emailMessage.To.Add(new MailboxAddress("", email));
        emailMessage.Subject = subject;
        emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html)
        {
            Text = message
        };

        using var client = new SmtpClient();

        await client.ConnectAsync("smtp.gmail.com", 587, SecureSocketOptions.StartTls, cancellationToken);
        await client.AuthenticateAsync("reenbittasktracker@gmail.com", "rads blth bejc sgeo", cancellationToken);
        await client.SendAsync(emailMessage, cancellationToken);

        await client.DisconnectAsync(true, cancellationToken);
    }
}
