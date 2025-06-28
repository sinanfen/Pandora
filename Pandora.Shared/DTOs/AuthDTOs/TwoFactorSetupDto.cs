namespace Pandora.Shared.DTOs.AuthDTOs;

public class TwoFactorSetupDto
{
    public string SecretKey { get; set; } = string.Empty;
    public string QrCodeUri { get; set; } = string.Empty;
    public string ManualEntryKey { get; set; } = string.Empty;
    public List<string> BackupCodes { get; set; } = new();
}

public class TwoFactorVerificationDto
{
    public string Code { get; set; } = string.Empty;
}

public class TwoFactorLoginDto
{
    public string TempToken { get; set; } = string.Empty; // Temporary token from initial login
    public string Code { get; set; } = string.Empty;
}

public class TwoFactorStatusDto
{
    public bool IsEnabled { get; set; }
    public DateTime? EnabledAt { get; set; }
    public int BackupCodesRemaining { get; set; }
}

public class TwoFactorToggleDto
{
    public bool Enable { get; set; }
    public string CurrentPassword { get; set; } = string.Empty;
    public string? VerificationCode { get; set; } // Required when enabling
} 