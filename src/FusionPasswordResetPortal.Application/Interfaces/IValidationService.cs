using FusionPasswordResetPortal.Domain.ValueObjects;

namespace FusionPasswordResetPortal.Application.Interfaces;

public interface IValidationService
{
    PasswordValidationResult ValidatePassword(string password);
    bool IsValidEmail(string email);
    bool IsRestrictedAccount(string userNameOrEmail);
}
