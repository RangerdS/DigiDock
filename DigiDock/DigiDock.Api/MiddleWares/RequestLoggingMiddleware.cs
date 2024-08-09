using DigiDock.Api.Services;
using DigiDock.Schema.Log;
using Hangfire;
using Serilog;
using System.Diagnostics;
using System.Text;

namespace DigiDock.Api.MiddleWares
{
    public class RequestLoggingMiddleware
    {
        private readonly RequestDelegate next;
        private readonly LogQueueService logQueueService;

        public RequestLoggingMiddleware(RequestDelegate next, LogQueueService logQueueService)
        {
            this.next = next;
            this.logQueueService = logQueueService;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var stopwatch = Stopwatch.StartNew();

            bool isHangfireRequest = IsHangfireRequest(context); 

            context.Request.EnableBuffering();
            var requestBody = await ReadRequestBodyAsync(context.Request);
            var ipAddress = context.Connection.RemoteIpAddress?.ToString();

            // fill here add rabbitMQ E0
            if (!isHangfireRequest)
            {
                var requestLogMessage = LogMessage.CreateRequestLog(ipAddress, context.Request.Method, context.Request.Path, requestBody);
                logQueueService.EnqueueLog("Information", requestLogMessage.ToString());
            }

            var originalBodyStream = context.Response.Body;
            using (var responseBody = new MemoryStream())
            {
                context.Response.Body = responseBody;

                await next(context);

                stopwatch.Stop();

                var responseBodyText = await ReadResponseBodyAsync(context.Response);
                if (!isHangfireRequest)
                {
                    var responseLogMessage = LogMessage.CreateResponseLog(context.Response.StatusCode, responseBodyText, stopwatch.ElapsedMilliseconds);
                    logQueueService.EnqueueLog("Information", responseLogMessage.ToString());
                }

                await responseBody.CopyToAsync(originalBodyStream);
            }
        }

        private bool IsHangfireRequest(HttpContext context)
        {
            // Hangfire job design
            return context.Request.Path.StartsWithSegments("/hangfire");
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