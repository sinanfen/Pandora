using System.Security.Cryptography;

namespace Pandora.Application.Security;

public static class AESEnvironmentHelper
{
    public static string GetOrCreateAesKey()
    {
        // Ortam değişkenlerinden Key'i al
        var key = Environment.GetEnvironmentVariable("AES_KEY");

        // Eğer Key yoksa, yeni bir tane oluştur ve ortam değişkenine kaydet
        if (string.IsNullOrEmpty(key))
        {
            key = GenerateRandomKey();
            Environment.SetEnvironmentVariable("AES_KEY", key);
        }
        // Anahtar uzunluğunu runtime'da kontrol et
        try
        {
            var keyBytes = Convert.FromBase64String(key);
            if (keyBytes.Length != 32)
                throw new InvalidOperationException("AES_KEY 32 bayt (256 bit) olmalıdır.");
        }
        catch (FormatException)
        {
            throw new InvalidOperationException("AES_KEY geçerli Base64 formatında değil.");
        }

        return key;
    }

    private static string GenerateRandomKey()
    {
        using var rng = RandomNumberGenerator.Create(); // Kriptografik RNG kullan
        byte[] key = new byte[32]; // 256 bit
        rng.GetBytes(key);
        return Convert.ToBase64String(key);
    }
}