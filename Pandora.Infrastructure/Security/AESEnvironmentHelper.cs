using System.Security.Cryptography;

namespace Pandora.Application.Security;

public static class AESEnvironmentHelper
{
    public static (string Key, string IV) GetOrCreateAesKeys()
    {
        // Ortam değişkenlerinden Key ve IV'yi al
        var key = Environment.GetEnvironmentVariable("AES_KEY");
        var iv = Environment.GetEnvironmentVariable("AES_IV");

        // Eğer biri eksikse, oluştur ve ortam değişkenine kaydet
        if (string.IsNullOrEmpty(key) || string.IsNullOrEmpty(iv))
        {
            key = GenerateRandomKey();
            iv = GenerateRandomIV();

            // Ortam değişkenlerine set et
            Environment.SetEnvironmentVariable("AES_KEY", key);
            Environment.SetEnvironmentVariable("AES_IV", iv);
        }

        return (key, iv);
    }

    private static string GenerateRandomKey()
    {
        using var aes = Aes.Create();
        aes.GenerateKey();
        return Convert.ToBase64String(aes.Key); // 32 byte Base64
    }

    private static string GenerateRandomIV()
    {
        using var aes = Aes.Create();
        aes.GenerateIV();
        return Convert.ToBase64String(aes.IV); // 16 byte Base64
    }

}
