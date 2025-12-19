using System;
using System.Linq;
using GFC.Core.Interfaces;
using GFC.Core.Models;

namespace GFC.Core.Services;

/// <summary>
/// Provides password validation that can be shared between server enforcement and UI hints.
/// </summary>
public class PasswordPolicy : IPasswordPolicy
{
    private static readonly string[] BannedPasswords = { "password", "123456", "qwerty", "letmein", "admin" };

    public int MinimumLength => 10;

    public string RequirementSummary => "Password must be at least 10 characters, include upper and lower case letters, and contain a number or symbol. It cannot contain the username or common passwords.";

    public PasswordValidationResult Validate(string username, string password)
    {
        if (string.IsNullOrWhiteSpace(password))
        {
            return PasswordValidationResult.Fail("Password is required.");
        }

        var normalizedPassword = password.Trim();
        if (normalizedPassword.Length < MinimumLength)
        {
            return PasswordValidationResult.Fail(RequirementSummary);
        }

        var hasUpper = normalizedPassword.Any(char.IsUpper);
        var hasLower = normalizedPassword.Any(char.IsLower);
        var hasDigit = normalizedPassword.Any(char.IsDigit);
        var hasSymbol = normalizedPassword.Any(ch => !char.IsLetterOrDigit(ch));

        if (!(hasUpper && hasLower && (hasDigit || hasSymbol)))
        {
            return PasswordValidationResult.Fail(RequirementSummary);
        }

        var normalizedUsername = username?.Trim().ToLowerInvariant();
        if (!string.IsNullOrEmpty(normalizedUsername) &&
            normalizedPassword.ToLowerInvariant().Contains(normalizedUsername))
        {
            return PasswordValidationResult.Fail("Password cannot contain the username.");
        }

        if (BannedPasswords.Any(banned =>
                normalizedPassword.Equals(banned, StringComparison.OrdinalIgnoreCase)))
        {
            return PasswordValidationResult.Fail("Password is too common.");
        }

        return PasswordValidationResult.Success();
    }
}
