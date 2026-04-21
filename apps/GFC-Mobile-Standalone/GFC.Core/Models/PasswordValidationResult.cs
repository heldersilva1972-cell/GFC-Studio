namespace GFC.Core.Models;

public class PasswordValidationResult
{
    public bool IsValid { get; init; }
    public string? ErrorMessage { get; init; }

    public static PasswordValidationResult Success() => new() { IsValid = true };

    public static PasswordValidationResult Fail(string? message) => new()
    {
        IsValid = false,
        ErrorMessage = message
    };
}
