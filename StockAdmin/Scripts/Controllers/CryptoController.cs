using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace StockAdmin.Scripts.Controllers;

public class CryptoController
{
    public string EncryptText(string text)
    {
        string encryptedText = Encrypt(text);

        return encryptedText;
    }

    private string Encrypt(string text)
    {
        try
        {
            var bytes = Encoding.UTF8.GetBytes(text);
            var encodedData = Convert.ToBase64String(bytes);
            return encodedData;
        }
        catch (Exception ex)
        {
            throw new Exception("Error in base64Encode: " + ex.Message);
        }
    }

    public string DecodeAndDecrypt(string strIn)
    {
        var encoder = new UTF8Encoding();
        var utf8Decode = encoder.GetDecoder();
        var bytes = Convert.FromBase64String(strIn);
        var charCount = utf8Decode.GetCharCount(bytes, 0, bytes.Length);
        var characters = new char[charCount];
        utf8Decode.GetChars(bytes, 0, bytes.Length, characters, 0);
        var result = new String(characters);
        return result;
    }
    
}