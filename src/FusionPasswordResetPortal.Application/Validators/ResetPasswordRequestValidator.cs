using FluentValidation;
using FusionPasswordResetPortal.Application.DTOs;
using FusionPasswordResetPortal.Application.Interfaces;

namespace FusionPasswordResetPortal.Application.Validators;

public class ResetPasswordRequestValidator : AbstractValidator<ResetPasswordRequestDto>
{
    public ResetPasswordRequestValidator(IValidationService validationService)
    {
        RuleFor(x => x.InstanceId).GreaterThan(0);
        RuleFor(x => x.UserName).NotEmpty().MaximumLength(150);
        RuleFor(x => x.Email).NotEmpty().EmailAddress();
        RuleFor(x => x.NewPassword)
            .NotEmpty()
            .Must(password => validationService.ValidatePassword(password).IsValid)
            .WithMessage("Password does not meet enterprise complexity requirements.");
        RuleFor(x => x.ConfirmPassword)
            .Equal(x => x.NewPassword)
            .WithMessage("Password and confirm password must match.");
    }
}
