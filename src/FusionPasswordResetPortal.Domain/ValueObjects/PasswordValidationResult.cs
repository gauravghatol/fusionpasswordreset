namespace FusionPasswordResetPortal.Domain.ValueObjects;

public sealed class PasswordValidationResult
{
    public bool IsValid { get; init; }
    public IReadOnlyCollection<string> Errors { get; init; } = Array.Empty<string>();
}
