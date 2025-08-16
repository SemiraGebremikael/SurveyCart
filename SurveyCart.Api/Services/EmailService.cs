using MailKit.Security;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Options;
using MimeKit;
using SurveyCart.Api.Settings;
using MailKit.Net.Smtp;


namespace SurveyCart.Api.Services;

public class EmailService(IOptions<EmailSettings> emailSettings, ILogger<EmailService> logger) : IEmailSender
{
    private readonly EmailSettings _emailSettings = emailSettings.Value;
    private readonly ILogger<EmailService> _logger;


    public async  Task SendEmailAsync(string email, string subject, string htmlMessage)
    {
        var message = new MimeMessage
        {
            Sender = MailboxAddress.Parse(_emailSettings.Email),
            Subject = subject,
        };

        if (string.IsNullOrWhiteSpace(email) || !IsValidEmail(email))
        {
            _logger.LogError("Invalid email address: {email}", email);
            throw new ArgumentException("Invalid email address", nameof(email));
        }

        message.To.Add(MailboxAddress.Parse(email));

        var builder = new BodyBuilder()
        {
            HtmlBody = htmlMessage
        };
        message.Body = builder.ToMessageBody();
        using var smtp = new SmtpClient();
        _logger.LogInformation("sending email to {email}", email);
        smtp.Connect(_emailSettings.Host, _emailSettings.Port, SecureSocketOptions.StartTls);

        smtp.Authenticate(_emailSettings.Email, _emailSettings.Password);

        await smtp.SendAsync(message);
        smtp.Disconnect(true);  
    }

    private bool IsValidEmail(string email)
    {
        try
        {
            var addr = new System.Net.Mail.MailAddress(email);
            return addr.Address == email;
        }
        catch
        {
            return false;
        }
    }
}
