using Pandora.Application.Security.Interfaces;
using System.Security.Authentication;
using System.Security.Cryptography;
using System.Text;

namespace Pandora.Application.Security;

public class SecurityService : IHasher, IEncryption
{
    private readonly byte[] _aesKey;

    public SecurityService()
    {
        // Environment variables'dan AES key değerlerini al
        _aesKey = Convert.FromBase64String(Environment.GetEnvironmentVariable("AES_KEY") ?? throw new InvalidOperationException("AES_KEY bulunamadı."));

        ThrowIfInvalidKey(_aesKey);
    }

    // IHasher Implementation
    public string HashPassword(string password, HashAlgorithmType algorithmType)
    {
        if (algorithmType == HashAlgorithmType.Sha256)
        {
            return HashPasswordSHA256(password);
        }
        return HashPasswordSHA512(password);
    }

    public bool VerifyHashedPassword(string hashedPassword, string plainTextPassword, HashAlgorithmType algorithmType)
    {
        if (algorithmType == HashAlgorithmType.Sha256)
        {
            return VerifyHashedPasswordSHA256(hashedPassword, plainTextPassword);
        }
        return VerifyHashedPasswordSHA512(hashedPassword, plainTextPassword);
    }

    private string HashPasswordSHA256(string password)
    {
        using (var sha256 = SHA256.Create())
        {
            var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            return BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();
        }
    }

    private string HashPasswordSHA512(string password)
    {
        using (var sha512 = SHA512.Create())
        {
            var hashedBytes = sha512.ComputeHash(Encoding.UTF8.GetBytes(password));
            return BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();
        }
    }

    private bool VerifyHashedPasswordSHA256(string hashedPassword, string plainTextPassword)
    {
        return HashPasswordSHA256(plainTextPassword) == hashedPassword;
    }

    private bool VerifyHashedPasswordSHA512(string hashedPassword, string plainTextPassword)
    {
        return HashPasswordSHA512(plainTextPassword) == hashedPassword;
    }

    // IEncryption Implementation (AES)
    public string Encrypt(string plainText)
    {
        using (Aes aesAlg = Aes.Create())
        {
            aesAlg.Key = _aesKey;

            // Rastgele IV oluştur
            aesAlg.GenerateIV();
            var iv = aesAlg.IV;

            ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

            using (var msEncrypt = new MemoryStream())
            {
                // IV'yi şifreli verinin başına ekle
                msEncrypt.Write(iv, 0, iv.Length);

                using (var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                using (var swEncrypt = new StreamWriter(csEncrypt))
                {
                    swEncrypt.Write(plainText);
                }

                return Convert.ToBase64String(msEncrypt.ToArray());
            }
        }
    }

    public string Decrypt(string cipherText)
    {
        var fullCipher = Convert.FromBase64String(cipherText);

        using (Aes aesAlg = Aes.Create())
        {
            aesAlg.Key = _aesKey;

            // Şifreli veriden IV'yi çıkar
            var iv = new byte[aesAlg.BlockSize / 8];
            Array.Copy(fullCipher, 0, iv, 0, iv.Length);
            aesAlg.IV = iv;

            var cipher = new byte[fullCipher.Length - iv.Length];
            Array.Copy(fullCipher, iv.Length, cipher, 0, cipher.Length);

            ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

            using (var msDecrypt = new MemoryStream(cipher))
            using (var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
            using (var srDecrypt = new StreamReader(csDecrypt))
            {
                return srDecrypt.ReadToEnd();
            }
        }
    }


    public void ThrowIfInvalidKey(byte[] key)
    {
        if (key.Length != 16 && key.Length != 24 && key.Length != 32)
        {
            throw new CryptographicException("Invalid AES key size. Key must be 16, 24, or 32 bytes.");
        }
    }

}