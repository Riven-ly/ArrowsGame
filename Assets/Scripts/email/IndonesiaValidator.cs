using UnityEngine;
using System.Text.RegularExpressions;

/// <summary>
/// 印尼输入验证器（印尼语）
/// </summary>
public class IndonesiaValidator
{
    /// <summary>
    /// 验证姓名：英文、无特殊字符、至少一个空格（名+姓）
    /// </summary>
    public static bool ValidateName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            return false;

        string cleaned = Regex.Replace(name.Trim(), @"\s+", " ");
        return Regex.IsMatch(cleaned, @"^[A-Za-z]+ [A-Za-z]+$");
    }

    /// <summary>
    /// 验证印尼手机号：支持 +62 或 62 前缀，国内部分长度10~12位
    /// </summary>
    public static bool ValidatePhone(string phone)
    {
        if (string.IsNullOrWhiteSpace(phone))
            return false;

        string digitsOnly = Regex.Replace(phone, @"[^\d]", "");

        // 剔除印尼国家码 62
        if (digitsOnly.Length >= 12 && digitsOnly.StartsWith("62"))
        {
            digitsOnly = digitsOnly.Substring(2);
        }

        // 国内号码长度应为10~12位
        return digitsOnly.Length >= 10 && digitsOnly.Length <= 12;
    }

    /// <summary>
    /// 验证邮箱标准格式
    /// </summary>
    public static bool ValidateAccount(string account)
    {
        return !string.IsNullOrWhiteSpace(account) && account.Trim().Length <= 128;
    }

    public static bool ValidateEmail(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
            return false;

        try
        {
            var addr = new System.Net.Mail.MailAddress(email);
            return addr.Address == email;
        }
        catch
        {
            return false;
        }
    }

    /// <summary>
    /// 综合验证，输出错误信息（印尼语）
    /// </summary>
    public static bool ValidateAll(string name, string phone, string email, string account, out string errorMessage)
    {
        if (!ValidateName(name))
        {
            errorMessage = "Nama tidak valid. (contoh: John Doe).";
            return false;
        }

        if (!ValidatePhone(phone))
        {
            errorMessage = "Nomor telepon tidak valid.";
            return false;
        }

        if (!ValidateEmail(email))
        {
            errorMessage = "Email tidak valid.";
            return false;
        }

        if (!ValidateAccount(account))
        {
            errorMessage = "Akun Venmo tidak valid.";
            return false;
        }
        errorMessage = null;
        return true;
    }
}