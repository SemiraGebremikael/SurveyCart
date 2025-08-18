using MailKit.Security;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Options;
using MimeKit;
using SurveyCart.Api.Settings;
using MailKit.Net.Smtp;


namespace SurveyCart.Api.Services;

public class EmailService : IEmailSender
{
    private readonly EmailSettings _emailSettings;
    private readonly ILogger<EmailService> _logger;

    public EmailService(IOptions<EmailSettings> emailSettings, ILogger<EmailService> logger)
    {
        _emailSettings = emailSettings.Value ?? throw new ArgumentNullException(nameof(emailSettings));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task SendEmailAsync(string email, string subject, string htmlMessage)
    {
        var message = new MimeMessage
        {
            Sender = new MailboxAddress("SurveyCart", _emailSettings.Email),
            Subject = subject,
        };

        if (string.IsNullOrWhiteSpace(email) || !IsValidEmail(email))
        {
            _logger.LogError("Invalid email address: {email}", email);
            throw new ArgumentException("Invalid email address", nameof(email));
        }


        if (string.IsNullOrWhiteSpace(_emailSettings.Host))
        {
            _logger.LogError("SMTP Host is missing in configuration.");
            throw new InvalidOperationException("SMTP Host is not configured.");
        }


        message.To.Add(new MailboxAddress("", email));

        var builder = new BodyBuilder { HtmlBody = htmlMessage };
        message.Body = builder.ToMessageBody();

        using var smtp = new SmtpClient();
        _logger.LogInformation("Sending email to {email}", email);

        await smtp.ConnectAsync(_emailSettings.Host, _emailSettings.Port, SecureSocketOptions.StartTls);
        await smtp.AuthenticateAsync(_emailSettings.Email, _emailSettings.Password);
        await smtp.SendAsync(message);
        await smtp.DisconnectAsync(true);
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
