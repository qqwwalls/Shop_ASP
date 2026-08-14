using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace Shop.Api.Controllers
{
    [ApiController]
    [ApiExplorerSettings(IgnoreApi = true)] // Щоб цей контролер не відображався у Swagger
    public class ErrorController : ControllerBase
    {
        [Route("/error")]
        public IActionResult HandleError()
        {
            var exceptionHandlerFeature = HttpContext.Features.Get<IExceptionHandlerFeature>();
            var exception = exceptionHandlerFeature?.Error;

            // Логування помилки можна додати тут (якщо потрібно)
            // logger.LogError(exception, "Global error caught");

            return Problem(
                detail: exception?.Message,
                title: "An unexpected error occurred in the application.",
                statusCode: 500
            );
        }
    }
}
