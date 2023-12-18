
using Serilog.Core;
using System.Text;


namespace WEB.Midleware
{
    public class LoggingMidleware
    {
        private readonly RequestDelegate _next;
        private readonly Logger _logger;
        public LoggingMidleware(RequestDelegate next, Logger logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            await _next(context);
            var statusCode = context.Response.StatusCode;
            if (statusCode < 200 || statusCode >= 300)
            {
                string logMessage = $" ---> request {context.Request.Path} {context.Response.StatusCode}";
                _logger.Information(logMessage);
            }
        }
    }
}
