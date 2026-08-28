using System.Text.RegularExpressions;

public static class AmericaValidator
{
    public static bool ValidateName(string name)
    {
        return !string.IsNullOrWhiteSpace(name) && Regex.IsMatch(Regex.Replace(name.Trim(), @"\s+", " "), @"^[A-Za-z]+ [A-Za-z]+$");
    }

    public static bool ValidatePhone(string phone)
    {
        if (string.IsNullOrWhiteSpace(phone))
        {
            return false;
        }

        string digitsOnly = Regex.Replace(phone, @"[^\d]", "");
        if (digitsOnly.StartsWith("1") && digitsOnly.Length == 11)
        {
            digitsOnly = digitsOnly.Substring(1);
        }

        return digitsOnly.Length == 10;
    }

    public static bool ValidateAccount(string account)
    {
        return !string.IsNullOrWhiteSpace(account) && account.Trim().Length <= 128;
    }

    public static bool ValidateAll(string name, string phone, string email, string account, out string errorMessage)
    {
        if (!ValidateName(name))
        {
            errorMessage = "Name is invalid.(example: John Doe).";
            return false;
        }

        if (!ValidatePhone(phone))
        {
            errorMessage = "Phone number is invalid.";
            return false;
        }

        if (!ValidateEmail(email))
        {
            errorMessage = "Email is invalid.";
            return false;
        }

        if (!ValidateAccount(account))
        {
            errorMessage = "Venmo account is invalid.";
            return false;
        }

        errorMessage = null;
        return true;
    }

    public static bool ValidateEmail(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
        {
            return false;
        }

        try
        {
            System.Net.Mail.MailAddress address = new System.Net.Mail.MailAddress(email);
            return address.Address == email;
        }
        catch
        {
            return false;
        }
    }
}
