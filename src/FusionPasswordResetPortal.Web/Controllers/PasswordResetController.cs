using FusionPasswordResetPortal.Application.Interfaces;
using FusionPasswordResetPortal.Web.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FusionPasswordResetPortal.Web.Controllers;

[Authorize(Policy = "PortalUser")]
public class PasswordResetController : Controller
{
    private readonly IFusionInstanceService _fusionInstanceService;
    private readonly IPasswordResetOrchestrator _orchestrator;

    public PasswordResetController(IFusionInstanceService fusionInstanceService, IPasswordResetOrchestrator orchestrator)
    {
        _fusionInstanceService = fusionInstanceService;
        _orchestrator = orchestrator;
    }

    [HttpGet]
    public async Task<IActionResult> Search(CancellationToken cancellationToken)
    {
        var model = new UserSearchViewModel
        {
            Instances = await _fusionInstanceService.GetActiveInstancesAsync(cancellationToken)
        };

        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> Search(UserSearchViewModel model, CancellationToken cancellationToken)
    {
        model.Instances = await _fusionInstanceService.GetActiveInstancesAsync(cancellationToken);
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var result = await _orchestrator.SearchUserAsync(model.InstanceId, model.UserNameOrEmail, cancellationToken);
        model.StatusMessage = result.Message;
        model.SearchResult = result.Data;

        if (!result.Succeeded)
        {
            ModelState.AddModelError(string.Empty, result.Message);
        }

        return View(model);
    }

    [HttpGet]
    public async Task<IActionResult> Reset(int instanceId, string userName, string email, CancellationToken cancellationToken)
    {
        var model = new ResetPasswordViewModel
        {
            InstanceId = instanceId,
            UserName = userName,
            Email = email,
            Instances = await _fusionInstanceService.GetActiveInstancesAsync(cancellationToken)
        };

        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> Reset(ResetPasswordViewModel model, CancellationToken cancellationToken)
    {
        model.Instances = await _fusionInstanceService.GetActiveInstancesAsync(cancellationToken);
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var result = await _orchestrator.ResetPasswordAsync(model.ToDto(), cancellationToken);
        if (!result.Succeeded)
        {
            ModelState.AddModelError(string.Empty, result.Message);
            return View(model);
        }

        TempData["SuccessMessage"] = result.Message;
        return RedirectToAction(nameof(Success));
    }

    [HttpGet]
    public IActionResult Success()
    {
        return View();
    }
}
