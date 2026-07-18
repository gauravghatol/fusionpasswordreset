namespace FusionPasswordResetPortal.Application.Interfaces;

public interface IUserContextService
{
    string GetUserEmail();
    string GetDisplayName();
    string GetObjectId();
    bool IsAuthenticated();
}
