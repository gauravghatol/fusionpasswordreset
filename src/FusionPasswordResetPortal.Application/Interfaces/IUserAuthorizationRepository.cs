namespace FusionPasswordResetPortal.Application.Interfaces;

public interface IUserAuthorizationRepository
{
    Task<bool> IsAuthorizedAsync(string userEmail, CancellationToken cancellationToken = default);
}
