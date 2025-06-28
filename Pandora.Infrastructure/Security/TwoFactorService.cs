using System.Security.Cryptography;
using System.Text;
using Pandora.Application.Interfaces.Security;

namespace Pandora.Infrastructure.Security;

public class TwoFactorService : ITwoFactorService
{
    private readonly int _timeStep = 30; // 30 seconds
    private readonly int _digits = 6; // 6 digit codes

    public string GenerateSecretKey()
    {
        var key = new byte[20]; // 160-bit key (recommended for TOTP)
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(key);
        return Convert.ToBase64String(key);
    }

    public string GenerateQrCodeUri(string secretKey, string userEmail, string appName = "Pandora")
    {
        // Convert Base64 secret to Base32 for TOTP compatibility
        var secretBytes = Convert.FromBase64String(secretKey);
        var base32Secret = ConvertToBase32(secretBytes);
        
        var encodedEmail = Uri.EscapeDataString(userEmail);
        var encodedAppName = Uri.EscapeDataString(appName);
        
        return $"otpauth://totp/{encodedAppName}:{encodedEmail}?secret={base32Secret}&issuer={encodedAppName}&digits={_digits}&period={_timeStep}";
    }

    public List<string> GenerateBackupCodes(int count = 10)
    {
        var codes = new List<string>();
        using var rng = RandomNumberGenerator.Create();
        
        for (int i = 0; i < count; i++)
        {
            var bytes = new byte[4];
            rng.GetBytes(bytes);
            var code = BitConverter.ToUInt32(bytes, 0).ToString("D8");
            codes.Add($"{code.Substring(0, 4)}-{code.Substring(4, 4)}");
        }
        
        return codes;
    }

    public bool VerifyCode(string secretKey, string code, int windowSize = 1)
    {
        if (string.IsNullOrEmpty(secretKey) || string.IsNullOrEmpty(code))
            return false;

        if (code.Length != _digits || !int.TryParse(code, out _))
            return false;

        try
        {
            var secretBytes = Convert.FromBase64String(secretKey);
            var currentTime = DateTimeOffset.UtcNow.ToUnixTimeSeconds() / _timeStep;

            // Check current time window and adjacent windows
            for (int i = -windowSize; i <= windowSize; i++)
            {
                var timeCounter = currentTime + i;
                var expectedCode = GenerateCode(secretBytes, timeCounter);
                
                if (expectedCode == code)
                    return true;
            }

            return false;
        }
        catch
        {
            return false;
        }
    }

    private string ConvertToBase32(byte[] input)
    {
        if (input == null || input.Length == 0)
            return string.Empty;

        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ234567";
        var result = new StringBuilder();
        
        for (int i = 0; i < input.Length; i += 5)
        {
            var chunk = new byte[5];
            var chunkLength = Math.Min(5, input.Length - i);
            Array.Copy(input, i, chunk, 0, chunkLength);
            
            // Convert 5 bytes (40 bits) to 8 base32 characters
            var buffer = 0UL;
            for (int j = 0; j < 5; j++)
            {
                buffer = (buffer << 8) | chunk[j];
            }
            
            for (int j = 0; j < 8; j++)
            {
                if (i * 8 / 5 + j >= (input.Length * 8 + 4) / 5)
                    break;
                    
                result.Append(chars[(int)((buffer >> (35 - j * 5)) & 0x1F)]);
            }
        }
        
        return result.ToString();
    }

    public bool VerifyBackupCode(List<string> backupCodes, string code)
    {
        if (backupCodes == null || string.IsNullOrEmpty(code))
            return false;

        return backupCodes.Contains(code, StringComparer.OrdinalIgnoreCase);
    }

    public List<string> RemoveUsedBackupCode(List<string> backupCodes, string usedCode)
    {
        var updatedCodes = new List<string>(backupCodes);
        updatedCodes.RemoveAll(c => string.Equals(c, usedCode, StringComparison.OrdinalIgnoreCase));
        return updatedCodes;
    }

    public string FormatSecretKeyForDisplay(string secretKey)
    {
        if (string.IsNullOrEmpty(secretKey))
            return string.Empty;

        // Convert Base64 secret to Base32 for manual entry
        var secretBytes = Convert.FromBase64String(secretKey);
        var base32Secret = ConvertToBase32(secretBytes);
        
        var formatted = new StringBuilder();
        for (int i = 0; i < base32Secret.Length; i += 4)
        {
            if (i > 0) formatted.Append(" ");
            var length = Math.Min(4, base32Secret.Length - i);
            formatted.Append(base32Secret.Substring(i, length));
        }
        
        return formatted.ToString();
    }

    private string GenerateCode(byte[] secretKey, long timeCounter)
    {
        var counterBytes = BitConverter.GetBytes(timeCounter);
        if (BitConverter.IsLittleEndian)
            Array.Reverse(counterBytes);

        using var hmac = new HMACSHA1(secretKey);
        var hash = hmac.ComputeHash(counterBytes);

        var offset = hash[hash.Length - 1] & 0x0F;
        var truncatedHash = ((hash[offset] & 0x7F) << 24) |
                           ((hash[offset + 1] & 0xFF) << 16) |
                           ((hash[offset + 2] & 0xFF) << 8) |
                           (hash[offset + 3] & 0xFF);

        var code = truncatedHash % (int)Math.Pow(10, _digits);
        return code.ToString($"D{_digits}");
    }
} 