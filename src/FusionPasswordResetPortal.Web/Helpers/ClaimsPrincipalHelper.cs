using System.Security.Claims;

namespace FusionPasswordResetPortal.Web.Helpers;

public class ClaimsPrincipalHelper
{
    public string GetEmail(ClaimsPrincipal user)
    {
        return user.FindFirstValue(ClaimTypes.Upn)
               ?? user.FindFirstValue(ClaimTypes.Email)
               ?? user.FindFirstValue("preferred_username")
               ?? string.Empty;
    }

    public string GetDisplayName(ClaimsPrincipal user)
    {
        return user.FindFirstValue("name") ?? user.Identity?.Name ?? string.Empty;
    }

    public string GetObjectId(ClaimsPrincipal user)
    {
        return user.FindFirstValue("oid") ?? string.Empty;
    }
}
