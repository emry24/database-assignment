﻿using System.Security.Cryptography;
using System.Text;

namespace Shared.Utilis;

public static class PasswordGenerator
{
    public static (string, string) GenerateSecurePassword(string password)
    {
        using var hmac = new HMACSHA256();
        var securityKey = Convert.ToBase64String(hmac.Key);
        var generatedPassword = Convert.ToBase64String(hmac.ComputeHash(Encoding.UTF8.GetBytes(password)));

        return (generatedPassword, securityKey);
    }
}
