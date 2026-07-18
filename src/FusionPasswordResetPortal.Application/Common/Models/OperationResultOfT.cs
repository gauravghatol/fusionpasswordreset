namespace FusionPasswordResetPortal.Application.Common.Models;

public class OperationResult<T> : OperationResult
{
    public T? Data { get; init; }

    public static OperationResult<T> Success(T data, string message, string? correlationId = null) =>
        new() { Succeeded = true, Message = message, Data = data, CorrelationId = correlationId };

    public new static OperationResult<T> Failure(string message, string? correlationId = null) =>
        new() { Succeeded = false, Message = message, CorrelationId = correlationId };
}
