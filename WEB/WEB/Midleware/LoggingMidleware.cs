
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
            this._next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var statusCode = context.Response.StatusCode;
            var responsePath = context.Request.Path;
            
            if(statusCode < 200 || statusCode > 300)
            {
                var stringBuilder = new StringBuilder();
                
                stringBuilder.Append(responsePath + statusCode);
                _logger.Information(stringBuilder.ToString());
            }
            await _next.Invoke(context);
        }
    }
}
