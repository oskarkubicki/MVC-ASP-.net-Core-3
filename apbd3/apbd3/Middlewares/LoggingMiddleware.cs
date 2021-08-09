using System.IO;
using System.Text;
using System.Threading.Tasks;
using apbd3.Services;
using Microsoft.AspNetCore.Http;

namespace apbd3.Middlewares
{
    public class LoggingMiddleware
    {
        private readonly RequestDelegate _next;

        public LoggingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext httpContext, IStudentsDbService service)
        {
            var log = "";

            if (httpContext.Request != null)
            {
                var path = httpContext.Request.Path;
                var method = httpContext.Request.Method;
                var queryString = httpContext.Request.QueryString.ToString();
                var bodyStr = "";

                httpContext.Request.EnableBuffering();

                using (var reader =
                    new StreamReader(httpContext.Request.Body, Encoding.UTF8, true, 1024, true))
                {
                    bodyStr = await reader.ReadToEndAsync();

                    httpContext.Request.Body.Position = 0;
                }

                log = path + " " + method + " " + queryString + " " + bodyStr + "\n";

                service.SaveLogData(log);
            }

            if (_next != null) await _next(httpContext);
        }
    }
}