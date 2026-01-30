using System.Net;
using System.Text.Json;

namespace Gorgonix_Back.Api.Middlewares;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionMiddleware> _logger;
    private readonly IHostEnvironment _env;

    public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger, IHostEnvironment env)
    {
        _next = next;
        _logger = logger;
        _env = env;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            // Deja pasar la petición al siguiente paso (Controller)
            await _next(context);
        }
        catch (Exception ex)
        {
            // Si algo explota en el controller, cae aquí
            _logger.LogError(ex, ex.Message);
            
            context.Response.ContentType = "application/json";
            
            // Definir el código de estado según el error
            context.Response.StatusCode = ex switch
            {
                KeyNotFoundException => (int)HttpStatusCode.NotFound,
                ArgumentException => (int)HttpStatusCode.BadRequest,
                InvalidOperationException => (int)HttpStatusCode.Conflict,
                UnauthorizedAccessException => (int)HttpStatusCode.Unauthorized,
                _ => (int)HttpStatusCode.InternalServerError
            };

            var response = new 
            {
                StatusCode = context.Response.StatusCode,
                Message = ex.Message,
                // En desarrollo mostramos el detalle técnico, en producción no
                Details = _env.IsDevelopment() ? ex.StackTrace?.ToString() : null
            };

            var json = JsonSerializer.Serialize(response);
            await context.Response.WriteAsync(json);
        }
    }
}