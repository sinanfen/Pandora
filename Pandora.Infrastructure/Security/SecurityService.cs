using Pandora.Application.Interfaces.Security;
using System.Security.Authentication;
using System.Security.Cryptography;
using System.Text;

namespace Pandora.Application.Security;

public class SecurityService : IHasher, IEncryption
{
    private readonly byte[] _key;
    private readonly byte[] _iv;

    public SecurityService(string key, string iv)
    {
        _key = Convert.FromBase64String(key);
        _iv = Convert.FromBase64String(iv);

        if (_key.Length != 32) // 256 bit = 32 byte
            throw new ArgumentException("The key must be 32 bytes (256 bits) long.");
        if (_iv.Length != 16) // 128 bit = 16 byte
            throw new ArgumentException("The IV must be 16 bytes (128 bits) long.");
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
        if (string.IsNullOrEmpty(plainText))
            throw new ArgumentNullException(nameof(plainText));

        using (Aes aesAlg = Aes.Create())
        {
            aesAlg.Key = _key;
            aesAlg.IV = _iv;
            aesAlg.Mode = CipherMode.CBC;
            aesAlg.Padding = PaddingMode.PKCS7;

            ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

            using (var msEncrypt = new MemoryStream())
            {
                using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                {
                    using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                    {
                        swEncrypt.Write(plainText);
                    }
                    return Convert.ToBase64String(msEncrypt.ToArray());
                }
            }
        }
    }

    public string Decrypt(string cipherText)
    {
        if (string.IsNullOrEmpty(cipherText))
            throw new ArgumentNullException(nameof(cipherText));

        var fullCipher = Convert.FromBase64String(cipherText);

        using (Aes aesAlg = Aes.Create())
        {
            aesAlg.Key = _key;
            aesAlg.IV = _iv;
            aesAlg.Mode = CipherMode.CBC;
            aesAlg.Padding = PaddingMode.PKCS7;

            ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

            using (MemoryStream msDecrypt = new MemoryStream(fullCipher))
            {
                using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                {
                    using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                    {
                        return srDecrypt.ReadToEnd();
                    }
                }
            }
        }
    }
}