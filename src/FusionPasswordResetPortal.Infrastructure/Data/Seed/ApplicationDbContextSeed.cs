using FusionPasswordResetPortal.Domain.Entities;

namespace FusionPasswordResetPortal.Infrastructure.Data.Seed;

public static class ApplicationDbContextSeed
{
    public static async Task SeedAsync(ApplicationDbContext context)
    {
        if (context.FusionResetPasswordInstances.Any())
        {
            return;
        }

        context.FusionResetPasswordInstances.AddRange(
            new FusionResetPasswordInstance
            {
                InstanceCode = "FIN-PROD",
                InstanceName = "Fusion Production",
                EnvironmentName = "Production",
                ApiBaseUrl = "https://apim.contoso.com/fusion/prod",
                IsActive = true,
                SortOrder = 1,
                CreatedBy = "seed"
            },
            new FusionResetPasswordInstance
            {
                InstanceCode = "FIN-UAT",
                InstanceName = "Fusion UAT",
                EnvironmentName = "UAT",
                ApiBaseUrl = "https://apim.contoso.com/fusion/uat",
                IsActive = true,
                SortOrder = 2,
                CreatedBy = "seed"
            });

        await context.SaveChangesAsync();
    }
}
