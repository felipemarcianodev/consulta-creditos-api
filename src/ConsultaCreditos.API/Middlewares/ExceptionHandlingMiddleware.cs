using System.Net;
using System.Text.Json;
using ConsultaCreditos.Domain.Exceptions;
using FluentValidation;

namespace ConsultaCreditos.API.Middlewares;

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
            await HandleExceptionAsync(context, ex);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        _logger.LogError(exception, "Erro não tratado: {Message}", exception.Message);

        var response = context.Response;
        response.ContentType = "application/json";

        var errorResponse = new ErrorResponse();

        switch (exception)
        {
            case ValidationException validationException:
                response.StatusCode = (int)HttpStatusCode.BadRequest;
                errorResponse.Message = "Erro de validação";
                errorResponse.Errors = validationException.Errors
                    .Select(e => e.ErrorMessage)
                    .ToList();
                break;

            case DomainException domainException:
                response.StatusCode = (int)HttpStatusCode.BadRequest;
                errorResponse.Message = domainException.Message;
                break;

            case ArgumentException argumentException:
                response.StatusCode = (int)HttpStatusCode.BadRequest;
                errorResponse.Message = argumentException.Message;
                break;

            default:
                response.StatusCode = (int)HttpStatusCode.InternalServerError;
                errorResponse.Message = "Erro interno do servidor";
                break;
        }

        var result = JsonSerializer.Serialize(errorResponse);
        await response.WriteAsync(result);
    }

    private class ErrorResponse
    {
        public string Message { get; set; } = string.Empty;
        public List<string>? Errors { get; set; }
    }
}
