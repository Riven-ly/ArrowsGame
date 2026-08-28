using System;
using System.Text;
using System.Security.Cryptography;
using UnityEngine;

public class EncryptDecodeUtils
{
    private const string AES_MODE = "AES/CBC/PKCS5Padding";

    /// <summary>
    /// 加密 - 与后端 communicationEncrypt 对应
    /// </summary>
    public static string CommunicationEncrypt(string content)
    {
        string key = StringMd5(Guid.NewGuid().ToString());
        string iv = StringMd5(Guid.NewGuid().ToString());
        if (iv.Length > 16)
        {
            iv = iv.Substring(0, 16);
        }

        byte[] encrypted = AesEncrypt(
            Encoding.UTF8.GetBytes(content),
            Encoding.UTF8.GetBytes(key),
            Encoding.UTF8.GetBytes(iv)
        );

        return Convert.ToBase64String(encrypted) + "." + key + "." + iv;
    }

    /// <summary>
    /// 解密 - 与后端 communicationDecrypt 对应
    /// </summary>
    public static string CommunicationDecrypt(string encrypted)
    {
        if (string.IsNullOrEmpty(encrypted))
            return "";

        string[] parts = encrypted.Split('.');
        if (parts.Length != 3)
            return "";

        byte[] cipherText = Convert.FromBase64String(parts[0]);
        byte[] decrypted = AesDecrypt(
            cipherText,
            Encoding.UTF8.GetBytes(parts[1]),
            Encoding.UTF8.GetBytes(parts[2])
        );

        return Encoding.UTF8.GetString(decrypted);
    }

    /// <summary>
    /// AES-CBC 加密
    /// </summary>
    private static byte[] AesEncrypt(byte[] data, byte[] key, byte[] iv)
    {
        using (AesManaged aes = new AesManaged())
        {
            aes.Mode = CipherMode.CBC;
            aes.Padding = PaddingMode.PKCS7; // Unity 中 PKCS7 等同于 Java 的 PKCS5Padding
            aes.Key = key;
            aes.IV = iv;

            ICryptoTransform encryptor = aes.CreateEncryptor();
            return encryptor.TransformFinalBlock(data, 0, data.Length);
        }
    }

    /// <summary>
    /// AES-CBC 解密
    /// </summary>
    private static byte[] AesDecrypt(byte[] data, byte[] key, byte[] iv)
    {
        using (AesManaged aes = new AesManaged())
        {
            aes.Mode = CipherMode.CBC;
            aes.Padding = PaddingMode.PKCS7;
            aes.Key = key;
            aes.IV = iv;

            ICryptoTransform decryptor = aes.CreateDecryptor();
            return decryptor.TransformFinalBlock(data, 0, data.Length);
        }
    }

    /// <summary>
    /// MD5 加密 - 与后端 stringMd5 对应
    /// </summary>
    private static string StringMd5(string input)
    {
        using (MD5 md5 = MD5.Create())
        {
            byte[] inputBytes = Encoding.UTF8.GetBytes(input);
            byte[] hashBytes = md5.ComputeHash(inputBytes);

            StringBuilder sb = new StringBuilder();
            foreach (byte b in hashBytes)
            {
                sb.Append(b.ToString("x2"));
            }
            return sb.ToString();
        }
    }
}