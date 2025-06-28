namespace Pandora.Application.Interfaces.Security;

public interface ITwoFactorService
{
    /// <summary>
    /// Generate a new TOTP secret key
    /// </summary>
    string GenerateSecretKey();
    
    /// <summary>
    /// Generate QR code URI for authenticator apps
    /// </summary>
    string GenerateQrCodeUri(string secretKey, string userEmail, string appName = "Pandora");
    
    /// <summary>
    /// Generate backup codes for emergency access
    /// </summary>
    List<string> GenerateBackupCodes(int count = 10);
    
    /// <summary>
    /// Verify TOTP code
    /// </summary>
    bool VerifyCode(string secretKey, string code, int windowSize = 1);
    
    /// <summary>
    /// Verify backup code
    /// </summary>
    bool VerifyBackupCode(List<string> backupCodes, string code);
    
    /// <summary>
    /// Remove used backup code from list
    /// </summary>
    List<string> RemoveUsedBackupCode(List<string> backupCodes, string usedCode);
    
    /// <summary>
    /// Format secret key for manual entry (groups of 4 characters)
    /// </summary>
    string FormatSecretKeyForDisplay(string secretKey);
} 