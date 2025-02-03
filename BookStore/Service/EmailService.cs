using System.Net;
using System.Net.Mail;

namespace BookStore.Services;

public interface IEmailService
{
    Task SendEmailAsync(string toEmail, string subject, string body);
}
// TODO: change smtp configs
public class EmailService : IEmailService
{
    private readonly string _smtpServer = "smtp.yourprovider.com"; // Replace with your SMTP server
    private readonly int _smtpPort = 587;  // SMTP port (587 for TLS)
    private readonly string _smtpUser = "your-email@example.com"; // Replace with your email address
    private readonly string _smtpPassword = "your-email-password"; // Replace with your email password
    private readonly string _fromEmail = "your-email@example.com"; // Replace with your from email address

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