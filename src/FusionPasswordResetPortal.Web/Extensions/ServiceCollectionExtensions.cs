using FusionPasswordResetPortal.Application.Interfaces;
using FusionPasswordResetPortal.Web.Helpers;
using FusionPasswordResetPortal.Web.Services;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Web;

namespace FusionPasswordResetPortal.Web.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddPortalAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddAuthentication(OpenIdConnectDefaults.AuthenticationScheme)
            .AddMicrosoftIdentityWebApp(configuration.GetSection("AzureAd"));

        services.Configure<OpenIdConnectOptions>(OpenIdConnectDefaults.AuthenticationScheme, options =>
        {
            options.TokenValidationParameters.NameClaimType = "name";
            options.TokenValidationParameters.RoleClaimType = "roles";
        });

        services.AddScoped<IUserContextService, UserContextService>();
        services.AddSingleton<ClaimsPrincipalHelper>();

        return services;
    }

    public static IServiceCollection AddPortalAuthorization(this IServiceCollection services)
    {
        services.AddAuthorization(options =>
        {
            options.FallbackPolicy = new AuthorizationPolicyBuilder()
                .RequireAuthenticatedUser()
                .Build();

            options.AddPolicy("PortalUser", policy =>
            {
                policy.RequireAuthenticatedUser();
                policy.RequireAssertion(context =>
                    context.User.HasClaim(claim => claim.Type == "roles") ||
                    context.User.HasClaim(claim => claim.Type == "groups"));
            });
        });

        return services;
    }

    public static IServiceCollection AddPortalMvc(this IServiceCollection services)
    {
        services.AddControllersWithViews(options =>
            {
                options.Filters.Add(new AutoValidateAntiforgeryTokenAttribute());
            });

        services.ConfigureApplicationCookie(options =>
        {
            options.Cookie.HttpOnly = true;
            options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
            options.Cookie.SameSite = SameSiteMode.Lax;
            options.SlidingExpiration = true;
            options.AccessDeniedPath = "/Home/AccessDenied";
        });

        return services;
    }
}
