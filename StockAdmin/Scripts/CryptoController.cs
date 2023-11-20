using System;
using System.Security.Cryptography;
using System.Text;

namespace StockAdmin.Scripts;

public class CryptoController
{
    private const string Key = "pSzVXzprWJwFALNR";
    
    
    public string EncryptText(string text)
    {
        string encryptedText = Encrypt(text, Key);
        
        return encryptedText;
    }

    public string GenerateAESKey(int length)
    {
        const string validChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
        StringBuilder keyBuilder = new StringBuilder();
        
        Random rnd = new Random();
        
        while (keyBuilder.Length < length)
        {
            keyBuilder.Append(validChars[rnd.Next(validChars.Length)]);
        }
        
        return keyBuilder.ToString();
    }
    private string Encrypt(string text, string key)
    {
        using Aes aesAlg = Aes.Create();
        aesAlg.Key = Encoding.UTF8.GetBytes(key);
        aesAlg.GenerateIV();

        ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

        byte[] cipherText;
        using (var msEncrypt = new System.IO.MemoryStream())
        {
            using (var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
            {
                using (var swEncrypt = new System.IO.StreamWriter(csEncrypt))
                {
                    swEncrypt.Write(text);
                }
                cipherText = msEncrypt.ToArray();
            }
        }

        return Convert.ToBase64String(aesAlg.IV) + ":" + Convert.ToBase64String(cipherText);
    }
    
}