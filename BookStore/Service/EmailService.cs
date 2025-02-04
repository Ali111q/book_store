using System.Net;
using System.Net.Mail;
using BookStore.Utils;

namespace BookStore.Services;

public interface IEmailService
{
    Task SendEmailAsync(string toEmail, string subject, string body);
}
// TODO: change smtp configs
public class EmailService : IEmailService
{
    private readonly string _smtpServer = Util.SmtpClient; // Replace with your SMTP server
    private readonly int _smtpPort = Util.SmtpPort;  // SMTP port (587 for TLS)
    private readonly string _smtpUser = Util.SmtpUser; // Replace with your email address
    private readonly string _smtpPassword = Util.SmtpPassword; // Replace with your email password
    private readonly string _fromEmail = Util.SmtpEmail; // Replace with your from email address

    public async Task SendEmailAsync(string toEmail, string subject, string body)
    {
        var mailMessage = new MailMessage
        {
            From = new MailAddress(_fromEmail),
            Subject = subject,
            Body = body,
            IsBodyHtml = true  // Set to false if you're sending plain text
        };

        mailMessage.To.Add(toEmail);

        using (var smtpClient = new SmtpClient(_smtpServer, _smtpPort))
        {
            smtpClient.Credentials = new NetworkCredential(_smtpUser, _smtpPassword);
            smtpClient.EnableSsl = true; // Set to true to use SSL/TLS
            await smtpClient.SendMailAsync(mailMessage);
        }
    }
}