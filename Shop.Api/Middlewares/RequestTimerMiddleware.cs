using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using System.Diagnostics;

namespace Shop.Api.Middlewares
{
    public class RequestTimerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<RequestTimerMiddleware> _logger;
 
        public RequestTimerMiddleware(RequestDelegate next, ILogger<RequestTimerMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }
 
        public async Task InvokeAsync(HttpContext context)
        {
            var stopwatch = Stopwatch.StartNew();
 
            _logger.LogInformation("Request started: {Path}", context.Request.Path);
 
            await _next(context);
 
            stopwatch.Stop();
            _logger.LogInformation("Request finished in {Ms} ms", stopwatch.ElapsedMilliseconds);
        }
    }
}
