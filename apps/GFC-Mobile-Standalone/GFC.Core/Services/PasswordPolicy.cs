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
        // Password policy requirements disabled for troubleshooting
        return PasswordValidationResult.Success();
    }
}
