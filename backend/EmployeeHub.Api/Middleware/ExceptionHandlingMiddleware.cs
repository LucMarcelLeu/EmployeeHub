using EmployeeHub.Api.Exceptions;
using EmployeeHub.Api.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

public sealed class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(
        RequestDelegate next,
        ILogger<ExceptionHandlingMiddleware> logger)
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
            _logger.LogError(ex,
                "Unhandled exception while processing {Path}",
                context.Request.Path);

            var response = CreateErrorResponse(ex, context.TraceIdentifier);
            context.Response.StatusCode = response.Status;
            context.Response.ContentType = "application/json";

            await context.Response.WriteAsJsonAsync(response);
        }
    }

    private static ErrorResponse CreateErrorResponse(
        Exception ex,
        string traceId)
    {
        return ex switch
        {
            ValidationException => new ErrorResponse
            {
                Status = StatusCodes.Status400BadRequest,
                Message = ex.Message,
                TraceId = traceId
            },

            NotFoundException => new ErrorResponse
            {
                Status = StatusCodes.Status404NotFound,
                Message = ex.Message,
                TraceId = traceId
            },

            ConflictException => new ErrorResponse
            {
                Status = StatusCodes.Status409Conflict,
                Message = ex.Message,
                TraceId = traceId
            },

            UnauthorizedAccessException => new ErrorResponse
            {
                Status = StatusCodes.Status401Unauthorized,
                Message = ex.Message,
                TraceId = traceId
            },

            _ => new ErrorResponse
            {
                Status = StatusCodes.Status500InternalServerError,
                Message = "An unexpected error occurred.",
                TraceId = traceId
            }
        };
    }
}