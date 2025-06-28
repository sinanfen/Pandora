namespace Pandora.Application.Interfaces;

public interface IEmailService
{
    /// <summary>
    /// Send email verification message
    /// </summary>
    Task<bool> SendEmailVerificationAsync(string toEmail, string userName, string verificationToken, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Send password reset email
    /// </summary>
    Task<bool> SendPasswordResetAsync(string toEmail, string userName, string resetToken, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Send security alert email
    /// </summary>
    Task<bool> SendSecurityAlertAsync(string toEmail, string userName, string alertMessage, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Send welcome email after successful verification
    /// </summary>
    Task<bool> SendWelcomeEmailAsync(string toEmail, string userName, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Send generic email
    /// </summary>
    Task<bool> SendEmailAsync(string toEmail, string subject, string htmlBody, CancellationToken cancellationToken = default);
} 