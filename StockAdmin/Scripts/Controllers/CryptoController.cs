using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace StockAdmin.Scripts.Controllers;

public class CryptoController
{
    private const string Key = "pSzVXzprWJwFALNR";


    public string EncryptText(string text)
    {
        string encryptedText = Encrypt(text);

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

    private string Encrypt(string text)
    {
        try
        {
            byte[] encData_byte = new byte[text.Length];
            encData_byte = Encoding.UTF8.GetBytes(text);
            string encodedData = Convert.ToBase64String(encData_byte);
            return encodedData;
        }
        catch (Exception ex)
        {
            throw new Exception("Error in base64Encode" + ex.Message);
        }
    }

    public string DecodeAndDecrypt(string strIn)
    {
        UTF8Encoding encoder = new UTF8Encoding();
        Decoder utf8Decode = encoder.GetDecoder();
        byte[] todecode_byte = Convert.FromBase64String(strIn);
        int charCount = utf8Decode.GetCharCount(todecode_byte, 0, todecode_byte.Length);
        char[] decoded_char = new char[charCount];
        utf8Decode.GetChars(todecode_byte, 0, todecode_byte.Length, decoded_char, 0);
        string result = new String(decoded_char);
        return result;
    }
    
}