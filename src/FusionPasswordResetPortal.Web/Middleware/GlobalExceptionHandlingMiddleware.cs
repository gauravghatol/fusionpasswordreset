using Serilog.Context;

namespace FusionPasswordResetPortal.Web.Middleware;

public class GlobalExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<GlobalExceptionHandlingMiddleware> _logger;

    public GlobalExceptionHandlingMiddleware(RequestDelegate next, ILogger<GlobalExceptionHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            using (LogContext.PushProperty("CorrelationId", context.TraceIdentifier))
            {
                await _next(context);
            }
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "Unhandled exception for request {TraceIdentifier}", context.TraceIdentifier);
            context.Response.Redirect("/Home/Error");
        }
    }
}
