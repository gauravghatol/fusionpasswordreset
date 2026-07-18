namespace FusionPasswordResetPortal.Application.Interfaces;

public interface IUserAuthorizationService
{
    Task<bool> IsAuthorizedAsync(string userEmail, CancellationToken cancellationToken = default);
}
