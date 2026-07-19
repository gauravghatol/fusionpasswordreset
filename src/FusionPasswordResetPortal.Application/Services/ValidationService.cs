using System.Text.RegularExpressions;
using FusionPasswordResetPortal.Application.Interfaces;
using FusionPasswordResetPortal.Domain.ValueObjects;

namespace FusionPasswordResetPortal.Application.Services;

public class ValidationService : IValidationService
{
    private static readonly Regex EmailRegex = new(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", RegexOptions.Compiled | RegexOptions.IgnoreCase);
    private static readonly string[] RestrictedAccounts = { "admin", "service", "integration", "sys" };

    public bool IsRestrictedAccount(string userNameOrEmail)
    {
        return RestrictedAccounts.Any(restricted =>
            userNameOrEmail.Contains(restricted, StringComparison.OrdinalIgnoreCase));
    }

    public bool IsValidEmail(string email) => EmailRegex.IsMatch(email);

    public PasswordValidationResult ValidatePassword(string password)
    {
        var errors = new List<string>();

        if (password.Length < 8) errors.Add("Password must be at least 8 characters.");
        if (!password.Any(char.IsUpper)) errors.Add("Password must contain an uppercase letter.");
        if (!password.Any(char.IsLower)) errors.Add("Password must contain a lowercase letter.");
        if (!password.Any(char.IsDigit)) errors.Add("Password must contain a number.");
        if (!password.Any(ch => !char.IsLetterOrDigit(ch))) errors.Add("Password must contain a special character.");

        return new PasswordValidationResult
        {
            IsValid = errors.Count == 0,
            Errors = errors
        };
    }
}
