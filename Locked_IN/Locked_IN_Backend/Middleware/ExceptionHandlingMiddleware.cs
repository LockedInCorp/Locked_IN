using System.Net;
using System.Text.Json;
using Locked_IN_Backend.Exceptions;

namespace Locked_IN_Backend.Middleware;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unhandled exception has occurred.");
            await HandleExceptionAsync(context, ex);
        }
    }

    private static Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        var code = HttpStatusCode.InternalServerError;
        var result = string.Empty;

        switch (exception)
        {
            case NotFoundException notFoundException:
                code = HttpStatusCode.NotFound;
                result = JsonSerializer.Serialize(new { message = notFoundException.Message });
                break;
            case ForbiddenException forbiddenException:
                code = HttpStatusCode.Forbidden;
                result = JsonSerializer.Serialize(new { message = forbiddenException.Message });
                break;
            case ConflictException conflictException:
                code = HttpStatusCode.Conflict;
                result = JsonSerializer.Serialize(new { message = conflictException.Message });
                break;
            case BadRequestException badRequestException:
                code = HttpStatusCode.BadRequest;
                result = JsonSerializer.Serialize(new { message = badRequestException.Message });
                break;
            case UnauthorizedException unauthorizedException:
                code = HttpStatusCode.Unauthorized;
                result = JsonSerializer.Serialize(new { message = unauthorizedException.Message });
                break;
            default:
                result = JsonSerializer.Serialize(new { message = exception.Message });
                break;
        }

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)code;

        return context.Response.WriteAsync(result);
    }
}
