using FusionPasswordResetPortal.Application.Interfaces;
using FusionPasswordResetPortal.Web.Helpers;

namespace FusionPasswordResetPortal.Web.Services;

public class UserContextService : IUserContextService
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly ClaimsPrincipalHelper _claimsPrincipalHelper;

    public UserContextService(IHttpContextAccessor httpContextAccessor, ClaimsPrincipalHelper claimsPrincipalHelper)
    {
        _httpContextAccessor = httpContextAccessor;
        _claimsPrincipalHelper = claimsPrincipalHelper;
    }

    public string GetDisplayName()
    {
        var user = _httpContextAccessor.HttpContext?.User;
        return user is null ? string.Empty : _claimsPrincipalHelper.GetDisplayName(user);
    }

    public string GetObjectId()
    {
        var user = _httpContextAccessor.HttpContext?.User;
        return user is null ? string.Empty : _claimsPrincipalHelper.GetObjectId(user);
    }

    public string GetUserEmail()
    {
        var user = _httpContextAccessor.HttpContext?.User;
        return user is null ? string.Empty : _claimsPrincipalHelper.GetEmail(user);
    }

    public bool IsAuthenticated() => _httpContextAccessor.HttpContext?.User?.Identity?.IsAuthenticated ?? false;
}
