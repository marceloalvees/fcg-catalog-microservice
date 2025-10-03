using Amazon.Runtime.Internal;
using Domain._Common.Exceptions;
using System.Net;

namespace Api._Common.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        private readonly ILogger<ExceptionMiddleware> _logger;

        private readonly IWebHostEnvironment _environment;

        public ExceptionMiddleware(
            RequestDelegate next,
            ILogger<ExceptionMiddleware> logger,
            IWebHostEnvironment environment)
        {
            _next = next;
            _logger = logger;
            _environment = environment;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var errorResponse = null as ErrorResponse;

            try
            {
                await _next(context);
            }
            catch (FCGDuplicateException duplicateException)
            {
                errorResponse = new ErrorResponse(duplicateException);
            }
            catch (FCGNotFoundException notFoundException)
            {
                errorResponse = new ErrorResponse(notFoundException);
            }
            catch (Exception exception)
            {
                errorResponse = new ErrorResponse(
                    HttpStatusCode.InternalServerError,
                    _environment.IsDevelopment()
                        ? exception.GetFullMessageString()
                        : "An unexpected error occurred."
                );

                _logger.LogError(
                    exception,
                    "Unhandled exception occurred. Path: {Path}, Method: {Method}, Error: {ErrorMessage}",
                    context.Request.Path,
                    context.Request.Method,
                    exception.GetFullMessageString()
                );
            }

            if (errorResponse is null) return;

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)errorResponse.StatusCode;

            await context.Response.WriteAsJsonAsync(errorResponse);
        }
    }
}
