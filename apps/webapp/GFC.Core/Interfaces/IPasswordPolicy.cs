using GFC.Core.Models;

namespace GFC.Core.Interfaces;

public interface IPasswordPolicy
{
    int MinimumLength { get; }

    string RequirementSummary { get; }

    PasswordValidationResult Validate(string username, string password);
}
