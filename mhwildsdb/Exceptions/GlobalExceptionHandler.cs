using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace mhwildsdb.Exceptions
{
    public sealed class GlobalExceptionHandler(
        IProblemDetailsService _problemDetails,
        ILogger<GlobalExceptionHandler> _logger) : IExceptionHandler
    {
        public async ValueTask<bool> TryHandleAsync(
            HttpContext httpContext,
            Exception exception,
            CancellationToken cancellationToken) 
        {
            _logger.LogError(exception, "Unhandled exception occurred. TraceId: {TraceId}", 
                httpContext.TraceIdentifier);

            var (statusCode, title) = MapException(exception);

            var problemDetails = new ProblemDetails
            {
                Status = statusCode,
                Title = title,
                Type = exception.GetType().Name,
                Detail = exception.Message,
                Instance = httpContext.Request.Path
            };

            problemDetails.Extensions["traceId"] = httpContext.TraceIdentifier;
            problemDetails.Extensions["timestamp"] = DateTime.UtcNow;

            return await _problemDetails.TryWriteAsync(new ProblemDetailsContext
            {
                HttpContext = httpContext,
                Exception = exception,
                ProblemDetails = problemDetails
            });
        }

        private static (int StatusCode, string Title) MapException(Exception exception) => exception switch
        {
            KeyNotFoundException => (StatusCodes.Status404NotFound, "Resource Not Found"),
            ArgumentException => (StatusCodes.Status400BadRequest, "Invalid Request"),
            UnauthorizedAccessException => (StatusCodes.Status403Forbidden, "Forbidden"),
            _ => (StatusCodes.Status500InternalServerError, "Server Error")
        };
    }
}
