namespace Pandora.Application.Security.Interfaces;

public interface IEncryption
{
    string Encrypt(string plainText);
    string Decrypt(string cipherText);
}