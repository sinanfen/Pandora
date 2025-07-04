using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Pandora.Application.Interfaces;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace Pandora.Infrastructure.Services;

public class EmailService : IEmailService
{
    private readonly IConfiguration _configuration;
    private readonly ILogger<EmailService> _logger;

    public EmailService(IConfiguration configuration, ILogger<EmailService> logger)
    {
        _configuration = configuration;
        _logger = logger;
    }

    public async Task<bool> SendEmailVerificationAsync(string toEmail, string userName, string verificationToken, CancellationToken cancellationToken = default)
    {
        var subject = "🔐 Pandora - Email Adresinizi Doğrulayın";
        var verificationUrl = $"{_configuration["AppSettings:BaseUrl"]}/auth/verify-email?token={verificationToken}";

        var htmlBody = GetEmailVerificationTemplate(userName, verificationUrl);

        return await SendEmailAsync(toEmail, subject, htmlBody, cancellationToken);
    }

    public async Task<bool> SendPasswordResetAsync(string toEmail, string userName, string resetToken, CancellationToken cancellationToken = default)
    {
        var subject = "🔑 Pandora - Şifre Sıfırlama Talebi";
        var resetUrl = $"{_configuration["AppSettings:BaseUrl"]}/auth/reset-password?token={resetToken}";

        var htmlBody = GetPasswordResetTemplate(userName, resetUrl);

        return await SendEmailAsync(toEmail, subject, htmlBody, cancellationToken);
    }

    public async Task<bool> SendSecurityAlertAsync(string toEmail, string userName, string alertMessage, CancellationToken cancellationToken = default)
    {
        var subject = "🚨 Pandora - Güvenlik Uyarısı";
        var htmlBody = GetSecurityAlertTemplate(userName, alertMessage);

        return await SendEmailAsync(toEmail, subject, htmlBody, cancellationToken);
    }

    public async Task<bool> SendWelcomeEmailAsync(string toEmail, string userName, CancellationToken cancellationToken = default)
    {
        var subject = "🎉 Pandora'ya Hoş Geldiniz!";
        var htmlBody = GetWelcomeTemplate(userName);

        return await SendEmailAsync(toEmail, subject, htmlBody, cancellationToken);
    }

    public async Task<bool> SendEmailAsync(string toEmail, string subject, string htmlBody, CancellationToken cancellationToken = default)
    {
        try
        {
            var smtpHost = _configuration["Email:SMTP:Host"];
            var smtpPort = int.Parse(_configuration["Email:SMTP:Port"]);
            var enableSsl = bool.Parse(_configuration["Email:SMTP:EnableSsl"]);
            var username = _configuration["Email:SMTP:Username"];
            var password = _configuration["Email:SMTP:Password"];
            var fromEmail = _configuration["Email:SMTP:FromEmail"];
            var fromName = _configuration["Email:SMTP:FromName"];

            _logger.LogInformation("Sending email to {Email} with subject {Subject}", toEmail, subject);
            _logger.LogInformation("SMTP Host: {Host}, Port: {Port}, SSL: {EnableSsl}, Username: {Username}, Password: {Password}, FromEmail: {FromEmail}, FromName: {FromName}",
                smtpHost, smtpPort, enableSsl, username, password, fromEmail, fromName);

            using var client = new SmtpClient(smtpHost, smtpPort)
            {
                Credentials = new NetworkCredential(username, password),
                EnableSsl = enableSsl,
                UseDefaultCredentials = false
            };

            var mailMessage = new MailMessage
            {
                From = new MailAddress(fromEmail, fromName),
                Subject = subject,
                Body = htmlBody,
                IsBodyHtml = true,
                BodyEncoding = Encoding.UTF8,
                SubjectEncoding = Encoding.UTF8
            };

            mailMessage.To.Add(toEmail);

            await client.SendMailAsync(mailMessage, cancellationToken);

            _logger.LogInformation("Email sent successfully to {Email} with subject {Subject}", toEmail, subject);
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to send email to {Email} with subject {Subject}", toEmail, subject);
            return false;
        }
    }

    private string GetEmailVerificationTemplate(string userName, string verificationUrl)
    {
        return $@"
<!DOCTYPE html>
<html lang='tr'>
<head>
    <meta charset='UTF-8'>
    <meta name='viewport' content='width=device-width, initial-scale=1.0'>
    <title>Email Doğrulama</title>
    <style>
        body {{ font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif; margin: 0; padding: 0; background-color: #f5f5f5; }}
        .container {{ max-width: 600px; margin: 40px auto; background: white; border-radius: 12px; box-shadow: 0 4px 20px rgba(0,0,0,0.1); overflow: hidden; }}
        .header {{ background: linear-gradient(135deg, #667eea 0%, #764ba2 100%); padding: 40px 30px; text-align: center; }}
        .header h1 {{ color: white; margin: 0; font-size: 28px; font-weight: 300; }}
        .content {{ padding: 40px 30px; }}
        .greeting {{ font-size: 18px; color: #333; margin-bottom: 20px; }}
        .message {{ color: #666; line-height: 1.6; margin-bottom: 30px; }}
        .button {{ display: inline-block; background: linear-gradient(135deg, #667eea 0%, #764ba2 100%); color: white; text-decoration: none; padding: 15px 30px; border-radius: 8px; font-weight: 500; margin: 20px 0; }}
        .footer {{ background: #f8f9fa; padding: 20px 30px; text-align: center; color: #666; font-size: 14px; }}
        .security-note {{ background: #fff3cd; border: 1px solid #ffeaa7; border-radius: 6px; padding: 15px; margin: 20px 0; color: #856404; }}
    </style>
</head>
<body>
    <div class='container'>
        <div class='header'>
            <div style='font-size: 48px; margin-bottom: 20px;'>🔐</div>
            <h1>Pandora Password Manager</h1>
        </div>
        <div class='content'>
            <div class='greeting'>Merhaba {userName},</div>
            <div class='message'>
                Pandora hesabınızı oluşturduğunuz için teşekkür ederiz! Güvenliğiniz bizim önceliğimizdir.
                <br><br>
                Hesabınızı aktifleştirmek için aşağıdaki butona tıklayarak email adresinizi doğrulayın:
            </div>
            <div style='text-align: center;'>
                <a href='{verificationUrl}' class='button'>✅ Email Adresimi Doğrula</a>
            </div>
            <div class='security-note'>
                <strong>Güvenlik Notu:</strong> Bu link 24 saat geçerlidir. Eğer bu isteği siz yapmadıysanız, bu emaili görmezden gelebilirsiniz.
            </div>
        </div>
        <div class='footer'>
            © 2024 Pandora Team. Tüm hakları saklıdır.
        </div>
    </div>
</body>
</html>";
    }

    private string GetPasswordResetTemplate(string userName, string resetUrl)
    {
        return $@"
<!DOCTYPE html>
<html lang='tr'>
<head>
    <meta charset='UTF-8'>
    <title>Şifre Sıfırlama</title>
    <style>
        body {{ font-family: 'Segoe UI', sans-serif; margin: 0; padding: 0; background-color: #f5f5f5; }}
        .container {{ max-width: 600px; margin: 40px auto; background: white; border-radius: 12px; box-shadow: 0 4px 20px rgba(0,0,0,0.1); }}
        .header {{ background: linear-gradient(135deg, #f093fb 0%, #f5576c 100%); padding: 40px 30px; text-align: center; color: white; }}
        .content {{ padding: 40px 30px; }}
        .button {{ display: inline-block; background: linear-gradient(135deg, #f093fb 0%, #f5576c 100%); color: white; text-decoration: none; padding: 15px 30px; border-radius: 8px; margin: 20px 0; }}
    </style>
</head>
<body>
    <div class='container'>
        <div class='header'>
            <div style='font-size: 48px; margin-bottom: 20px;'>🔑</div>
            <h1>Şifre Sıfırlama</h1>
        </div>
        <div class='content'>
            <p>Merhaba {userName},</p>
            <p>Pandora hesabınız için şifre sıfırlama talebinde bulundunuz.</p>
            <div style='text-align: center;'>
                <a href='{resetUrl}' class='button'>🔒 Yeni Şifre Belirle</a>
            </div>
            <p><strong>Bu link 1 saat geçerlidir.</strong></p>
        </div>
    </div>
</body>
</html>";
    }

    private string GetWelcomeTemplate(string userName)
    {
        return $@"
<!DOCTYPE html>
<html lang='tr'>
<head>
    <meta charset='UTF-8'>
    <title>Hoş Geldiniz</title>
    <style>
        body {{ font-family: 'Segoe UI', sans-serif; margin: 0; padding: 0; background-color: #f5f5f5; }}
        .container {{ max-width: 600px; margin: 40px auto; background: white; border-radius: 12px; box-shadow: 0 4px 20px rgba(0,0,0,0.1); }}
        .header {{ background: linear-gradient(135deg, #43e97b 0%, #38f9d7 100%); padding: 40px 30px; text-align: center; color: white; }}
        .content {{ padding: 40px 30px; }}
    </style>
</head>
<body>
    <div class='container'>
        <div class='header'>
            <div style='font-size: 48px; margin-bottom: 20px;'>🎉</div>
            <h1>Hoş Geldiniz!</h1>
        </div>
        <div class='content'>
            <p>Tebrikler {userName}!</p>
            <p>Email adresinizi başarıyla doğruladınız ve Pandora ailesine katıldınız!</p>
        </div>
    </div>
</body>
</html>";
    }

    private string GetSecurityAlertTemplate(string userName, string alertMessage)
    {
        return $@"
<!DOCTYPE html>
<html lang='tr'>
<head>
    <meta charset='UTF-8'>
    <title>Güvenlik Uyarısı</title>
    <style>
        body {{ font-family: 'Segoe UI', sans-serif; margin: 0; padding: 0; background-color: #f5f5f5; }}
        .container {{ max-width: 600px; margin: 40px auto; background: white; border-radius: 12px; }}
        .header {{ background: linear-gradient(135deg, #ff6b6b 0%, #ee5a24 100%); padding: 40px 30px; text-align: center; color: white; }}
        .content {{ padding: 40px 30px; }}
        .alert {{ background: #f8d7da; border: 1px solid #f5c6cb; border-radius: 6px; padding: 15px; margin: 20px 0; color: #721c24; }}
    </style>
</head>
<body>
    <div class='container'>
        <div class='header'>
            <div style='font-size: 48px; margin-bottom: 20px;'>🚨</div>
            <h1>Güvenlik Uyarısı</h1>
        </div>
        <div class='content'>
            <p>Sayın {userName},</p>
            <div class='alert'>
                <strong>Önemli Güvenlik Bildirimi:</strong><br>
                {alertMessage}
            </div>
        </div>
    </div>
</body>
</html>";
    }
}