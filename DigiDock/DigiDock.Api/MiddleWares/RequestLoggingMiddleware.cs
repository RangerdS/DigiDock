using Hangfire;
using Serilog;
using System.Diagnostics;
using System.Text;

namespace DigiDock.Api.MiddleWares
{
    public class RequestLoggingMiddleware
    {
        private readonly RequestDelegate next;

        public RequestLoggingMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var stopwatch = Stopwatch.StartNew();

            context.Request.EnableBuffering();
            var requestBody = await ReadRequestBodyAsync(context.Request);
            BackgroundJob.Enqueue(() => Log.Information("HTTP Request: {Method} {Path} {Body}",
                context.Request.Method,
                context.Request.Path,
                requestBody));

            var originalBodyStream = context.Response.Body;
            using (var responseBody = new MemoryStream())
            {
                context.Response.Body = responseBody;

                await next(context);

                stopwatch.Stop();

                var responseBodyText = await ReadResponseBodyAsync(context.Response);
                BackgroundJob.Enqueue(() => Log.Information("HTTP Response: {StatusCode} {Body} in {ElapsedMilliseconds}ms",
                    context.Response.StatusCode,
                    responseBodyText,
                    stopwatch.ElapsedMilliseconds));

                await responseBody.CopyToAsync(originalBodyStream);
            }
        }

        private async Task<string> ReadRequestBodyAsync(HttpRequest request)
        {
            request.Body.Position = 0;
            using (var reader = new StreamReader(request.Body, Encoding.UTF8, true, 1024, true))
            {
                var body = await reader.ReadToEndAsync();
                request.Body.Position = 0;
                return body;
            }
        }

        private async Task<string> ReadResponseBodyAsync(HttpResponse response)
        {
            response.Body.Position = 0;
            using (var reader = new StreamReader(response.Body, Encoding.UTF8, true, 1024, true))
            {
                var body = await reader.ReadToEndAsync();
                response.Body.Position = 0;
                return body;
            }
        }
    }
}