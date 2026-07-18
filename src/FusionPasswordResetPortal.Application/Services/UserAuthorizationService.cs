using FusionPasswordResetPortal.Application.Interfaces;

namespace FusionPasswordResetPortal.Application.Services;

public class UserAuthorizationService : IUserAuthorizationService
{
    private readonly IUserAuthorizationRepository _repository;

    public UserAuthorizationService(IUserAuthorizationRepository repository)
    {
        _repository = repository;
    }

    public Task<bool> IsAuthorizedAsync(string userEmail, CancellationToken cancellationToken = default)
    {
        return _repository.IsAuthorizedAsync(userEmail, cancellationToken);
    }
}
