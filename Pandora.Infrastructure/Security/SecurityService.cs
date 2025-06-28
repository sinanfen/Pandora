using Pandora.Application.Interfaces.Security;
using System.Security.Authentication;
using System.Security.Cryptography;
using System.Text;

namespace Pandora.Application.Security;

public class SecurityService : IHasher, IEncryption
{
    private readonly byte[] _key;
    private const int IV_SIZE = 16; // AES bloğu 128 bit (16 byte)

    public SecurityService(string key)
    {
        _key = Convert.FromBase64String(key);
        if (_key.Length != 32)
            throw new ArgumentException("Key must be 256 bits (32 bytes).");
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
            aesAlg.GenerateIV();
            aesAlg.Mode = CipherMode.CBC;
            aesAlg.Padding = PaddingMode.PKCS7;

            using (var encryptor = aesAlg.CreateEncryptor())
            using (var ms = new MemoryStream())
            {
                ms.Write(aesAlg.IV, 0, IV_SIZE); // IV'i ön ek olarak ekle
                using (var cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
                using (var sw = new StreamWriter(cs, Encoding.UTF8))
                {
                    sw.Write(plainText);
                }
                return Convert.ToBase64String(ms.ToArray());
            }
        }
    }

    public string Decrypt(string cipherText)
    {
        if (string.IsNullOrEmpty(cipherText))
            throw new ArgumentNullException(nameof(cipherText));

        byte[] cipherBytes;
        try
        {
            cipherBytes = Convert.FromBase64String(cipherText);
        }
        catch (FormatException)
        {
            throw new ArgumentException("Invalid Base64 format.", nameof(cipherText));
        }

        if (cipherBytes.Length < IV_SIZE)
            throw new ArgumentException("Encrypted data is too short.");

        using (Aes aes = Aes.Create())
        {
            aes.Key = _key;
            aes.Mode = CipherMode.CBC;
            aes.Padding = PaddingMode.PKCS7;

            // IV ve ciphertext'i ayır
            byte[] iv = new byte[IV_SIZE];
            Array.Copy(cipherBytes, 0, iv, 0, IV_SIZE);
            aes.IV = iv;

            byte[] encryptedData = new byte[cipherBytes.Length - IV_SIZE];
            Array.Copy(cipherBytes, IV_SIZE, encryptedData, 0, encryptedData.Length);

            using (var decryptor = aes.CreateDecryptor())
            using (var ms = new MemoryStream(encryptedData))
            using (var cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read))
            using (var sr = new StreamReader(cs, Encoding.UTF8))
            {
                try
                {
                    return sr.ReadToEnd();
                }
                catch (CryptographicException ex)
                {
                    throw new CryptographicException("Decryption failed. Key or data is corrupted.", ex);
                }
            }
        }
    }
}