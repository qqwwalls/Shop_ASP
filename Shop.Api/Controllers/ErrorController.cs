using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace Shop.Api.Controllers
{
    [ApiController]
    [ApiExplorerSettings(IgnoreApi = true)] 
    public class ErrorController : ControllerBase
    {
        [Route("/error")]
        public IActionResult HandleError(System.Threading.CancellationToken cancellationToken)
        {
            var exceptionHandlerFeature = HttpContext.Features.Get<IExceptionHandlerFeature>();
            var exception = exceptionHandlerFeature?.Error;

            return Problem(
                detail: exception?.Message,
                title: "An unexpected error occurred in the application.",
                statusCode: 500
            );
        }
    }
}
