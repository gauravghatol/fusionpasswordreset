namespace FusionPasswordResetPortal.Application.Common.Models;

public class OperationResult
{
    public bool Succeeded { get; init; }
    public string Message { get; init; } = string.Empty;
    public string? CorrelationId { get; init; }

    public static OperationResult Success(string message, string? correlationId = null) =>
        new() { Succeeded = true, Message = message, CorrelationId = correlationId };

    public static OperationResult Failure(string message, string? correlationId = null) =>
        new() { Succeeded = false, Message = message, CorrelationId = correlationId };
}
