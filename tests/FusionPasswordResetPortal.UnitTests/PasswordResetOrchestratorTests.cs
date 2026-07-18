using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using FusionPasswordResetPortal.Application.DTOs;
using FusionPasswordResetPortal.Application.Interfaces;
using FusionPasswordResetPortal.Application.Services;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;

namespace FusionPasswordResetPortal.UnitTests;

public class PasswordResetOrchestratorTests
{
    [Fact]
    public async Task ResetPasswordAsync_ShouldFail_WhenRequesterIsUnauthorized()
    {
        var oracleApi = new Mock<IOracleApiService>();
        var authorization = new Mock<IUserAuthorizationService>();
        var userContext = new Mock<IUserContextService>();
        var validation = new Mock<IValidationService>();
        var audit = new Mock<IAuditService>();
        var validator = new Mock<IValidator<ResetPasswordRequestDto>>();

        userContext.Setup(x => x.GetUserEmail()).Returns("user@contoso.com");
        authorization.Setup(x => x.IsAuthorizedAsync("user@contoso.com", It.IsAny<CancellationToken>())).ReturnsAsync(false);
        validation.Setup(x => x.IsRestrictedAccount(It.IsAny<string>())).Returns(false);
        validator.Setup(x => x.ValidateAsync(It.IsAny<ValidationContext<ResetPasswordRequestDto>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult());

        var orchestrator = new PasswordResetOrchestrator(
            oracleApi.Object,
            authorization.Object,
            userContext.Object,
            validation.Object,
            audit.Object,
            validator.Object,
            NullLogger<PasswordResetOrchestrator>.Instance);

        var result = await orchestrator.ResetPasswordAsync(new ResetPasswordRequestDto
        {
            InstanceId = 1,
            UserName = "target.user",
            Email = "target@contoso.com",
            NewPassword = "Str0ng!Pass",
            ConfirmPassword = "Str0ng!Pass"
        });

        result.Succeeded.Should().BeFalse();
    }
}
