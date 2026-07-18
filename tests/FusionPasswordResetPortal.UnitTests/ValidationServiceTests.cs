using FluentAssertions;
using FusionPasswordResetPortal.Application.Services;

namespace FusionPasswordResetPortal.UnitTests;

public class ValidationServiceTests
{
    private readonly ValidationService _service = new();

    [Fact]
    public void ValidatePassword_ShouldRejectWeakPassword()
    {
        var result = _service.ValidatePassword("weak");

        result.IsValid.Should().BeFalse();
        result.Errors.Should().NotBeEmpty();
    }

    [Fact]
    public void ValidatePassword_ShouldAcceptStrongPassword()
    {
        var result = _service.ValidatePassword("Str0ng!Pass");

        result.IsValid.Should().BeTrue();
    }
}
