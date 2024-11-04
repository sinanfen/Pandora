namespace Pandora.Application.Interfaces.Security;

public interface IEncryption
{
    string Encrypt(string plainText);
    string Decrypt(string cipherText);
}