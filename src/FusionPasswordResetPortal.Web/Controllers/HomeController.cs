using FusionPasswordResetPortal.Application.Interfaces;
using FusionPasswordResetPortal.Web.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FusionPasswordResetPortal.Web.Controllers;

[Authorize(Policy = "PortalUser")]
public class HomeController : Controller
{
    private readonly IFusionInstanceService _fusionInstanceService;
    private readonly IUserContextService _userContextService;

    public HomeController(IFusionInstanceService fusionInstanceService, IUserContextService userContextService)
    {
        _fusionInstanceService = fusionInstanceService;
        _userContextService = userContextService;
    }

    [AllowAnonymous]
    public IActionResult Index()
    {
        if (User.Identity?.IsAuthenticated == true)
        {
            return RedirectToAction(nameof(Dashboard));
        }

        return View();
    }

    public async Task<IActionResult> Dashboard(CancellationToken cancellationToken)
    {
        var viewModel = new DashboardViewModel
        {
            DisplayName = _userContextService.GetDisplayName(),
            Email = _userContextService.GetUserEmail(),
            Instances = await _fusionInstanceService.GetActiveInstancesAsync(cancellationToken)
        };

        return View(viewModel);
    }

    [AllowAnonymous]
    public IActionResult AccessDenied() => View();

    [AllowAnonymous]
    public IActionResult Error() => View();
}
