using System.Net;
using AppLevelAuthorization.Application.Exceptions;

namespace AppLevelAuthorization.Api.Middlewares;

public class CustomExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<CustomExceptionMiddleware> _logger;

    public CustomExceptionMiddleware(RequestDelegate next, ILogger<CustomExceptionMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }
    
    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }

    private Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        HttpStatusCode code;
        string message;
        
        
        switch (exception)
        {
            case ForbiddenException:
                code = HttpStatusCode.Forbidden;
                message = string.Empty;
                break;
            default:
                code = HttpStatusCode.InternalServerError;
                message = "Something went wrong";
                _logger.LogError("Something went wrong: {exception}", exception);
                break;
        }

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)code;

        return context.Response.WriteAsync(message);
    }
}