using UnityEngine;
using System.Text.RegularExpressions;

/// <summary>
/// 巴西输入验证器（印尼语）
/// </summary>
public class BrazilValidator
{
    public static bool ValidateName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            return false;

        string cleaned = Regex.Replace(name.Trim(), @"\s+", " ");
        return Regex.IsMatch(cleaned, @"^[A-Za-z]+ [A-Za-z]+$");
    }

    public static bool ValidatePhone(string phone)
    {
        if (string.IsNullOrWhiteSpace(phone))
            return false;

        string digitsOnly = Regex.Replace(phone, @"[^\d]", "");

        if (digitsOnly.Length >= 12 && digitsOnly.StartsWith("55"))
        {
            digitsOnly = digitsOnly.Substring(2);
        }

        if (digitsOnly.Length != 10 && digitsOnly.Length != 11)
            return false;

        if (digitsOnly.Length == 11 && digitsOnly[0] != '9')
            return false;

        return true;
    }

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
    /// 巴西
    /// </summary>
    public static bool ValidateAll(string name, string phone, string email, string account, out string errorMessage)
    {
        if (!ValidateName(name))
        {
            errorMessage = "Nome inválido. (exemplo: John Doe).";
            return false;
        }

        if (!ValidatePhone(phone))
        {
            errorMessage = "Número de telefone inválido.";
            return false;
        }

        if (!ValidateEmail(email))
        {
            errorMessage = "E‑mail inválido.";
            return false;
        }

        if (!ValidateAccount(account))
        {
            errorMessage = "Conta Venmo inválida.";
            return false;
        }
        errorMessage = null;
        return true;
    }
}