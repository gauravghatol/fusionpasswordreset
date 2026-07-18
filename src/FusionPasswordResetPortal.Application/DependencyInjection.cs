using FluentValidation;
using FluentValidation.AspNetCore;
using FusionPasswordResetPortal.Application.Interfaces;
using FusionPasswordResetPortal.Application.Services;
using FusionPasswordResetPortal.Application.Validators;
using Microsoft.Extensions.DependencyInjection;

namespace FusionPasswordResetPortal.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<IValidationService, ValidationService>();
        services.AddScoped<IFusionInstanceService, FusionInstanceService>();
        services.AddScoped<IUserAuthorizationService, UserAuthorizationService>();
        services.AddScoped<IAuditService, AuditService>();
        services.AddScoped<IPasswordResetOrchestrator, PasswordResetOrchestrator>();

        services.AddValidatorsFromAssemblyContaining<ResetPasswordRequestValidator>(ServiceLifetime.Scoped);
        services.AddFluentValidationAutoValidation();

        return services;
    }
}
