using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;
using System;

namespace Shop.Api.HandlerExceptions
{
    public class GlobalExceptionHandler : IExceptionHandler
    {
        private readonly ILogger<GlobalExceptionHandler> _logger;
        private readonly IProblemDetailsService _problemDetailsService;

        public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger, IProblemDetailsService problemDetailsService)
        {
            _logger = logger;
            _problemDetailsService = problemDetailsService;
        }

        public async ValueTask<bool> TryHandleAsync(
            HttpContext httpContext, 
            Exception exception, 
            CancellationToken cancellationToken)
        {
            _logger.LogError(exception, "Exception occurred: {Message}", exception.Message);

            // Визначаємо статус-код та заголовок залежно від типу помилки
            var (statusCode, title) = exception switch
            {
                NotFoundException => (StatusCodes.Status404NotFound, "Not Found"),
                ValidationAppException => (StatusCodes.Status400BadRequest, "Validation Error"),
                _ => (StatusCodes.Status500InternalServerError, "Server Error")
            };

            // Формуємо стандартну відповідь Problem Details
            var problemDetails = new ProblemDetails
            {
                Status = statusCode,
                Title = title,
                Detail = exception.Message,
                Instance = $"{httpContext.Request.Method} {httpContext.Request.Path}"
            };

            // Якщо це помилка валідації, додаємо список помилок
            if (exception is ValidationAppException validationEx)
            {
                problemDetails.Extensions.Add("errors", validationEx.Errors);
            }

            httpContext.Response.StatusCode = statusCode;

            return await _problemDetailsService.TryWriteAsync(new ProblemDetailsContext
            {
                HttpContext = httpContext,
                ProblemDetails = problemDetails,
                Exception = exception
            });
        }
    }
}
