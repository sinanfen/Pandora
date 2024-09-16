using Pandora.Application.Security.Interfaces;
using System.Security.Authentication;
using System.Security.Cryptography;
using System.Text;

namespace Pandora.Application.Security;

public class SecurityService : IHasher, IEncryption
{
    private readonly byte[] _aesKey;
    private readonly byte[] _aesIv;

    public SecurityService()
    {
        // Use secure key management (e.g., environment variables) in production
        _aesKey = Encoding.UTF8.GetBytes("your-32-byte-aes-key-1234567890123456");
        _aesIv = Encoding.UTF8.GetBytes("your-16-byte-iv-here");
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
            aesAlg.IV = _aesIv;

            ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

            using (var msEncrypt = new MemoryStream())
            {
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
        using (Aes aesAlg = Aes.Create())
        {
            aesAlg.Key = _aesKey;
            aesAlg.IV = _aesIv;

            ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

            using (var msDecrypt = new MemoryStream(Convert.FromBase64String(cipherText)))
            {
                using (var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                using (var srDecrypt = new StreamReader(csDecrypt))
                {
                    return srDecrypt.ReadToEnd();
                }
            }
        }
    }
}