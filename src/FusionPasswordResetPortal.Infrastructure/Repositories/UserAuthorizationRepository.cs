using FusionPasswordResetPortal.Application.Interfaces;
using FusionPasswordResetPortal.Infrastructure.Data;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace FusionPasswordResetPortal.Infrastructure.Repositories;

public class UserAuthorizationRepository : IUserAuthorizationRepository
{
    private readonly ApplicationDbContext _context;
    private readonly IConfiguration _configuration;

    public UserAuthorizationRepository(ApplicationDbContext context, IConfiguration configuration)
    {
        _context = context;
        _configuration = configuration;
    }

    public async Task<bool> IsAuthorizedAsync(string userEmail, CancellationToken cancellationToken = default)
    {
        var query = _configuration["Authorization:SqlQuery"] ??
                    "SELECT CASE WHEN EXISTS (SELECT 1 FROM TblFusionUserPWDRest WHERE RequesterEmail = @email AND IsAuthorized = 1) THEN CAST(1 AS BIT) ELSE CAST(0 AS BIT) END";

        await using var connection = _context.Database.GetDbConnection();
        if (connection.State != System.Data.ConnectionState.Open)
        {
            await connection.OpenAsync(cancellationToken);
        }

        await using var command = connection.CreateCommand();
        command.CommandText = query;
        command.Parameters.Add(new SqlParameter("@email", userEmail));
        var result = await command.ExecuteScalarAsync(cancellationToken);

        return result is bool isAuthorized && isAuthorized;
    }
}
