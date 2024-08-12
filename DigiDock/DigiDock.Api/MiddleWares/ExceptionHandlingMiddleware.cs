using DigiDock.Base.Responses;
using Hangfire;
using Serilog;
using System.Net;

namespace DigiDock.Api.MiddleWares
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate next;

        public ExceptionHandlingMiddleware(RequestDelegate next)
        {
            this.next = next ?? throw new ArgumentNullException(nameof(next));
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            BackgroundJob.Enqueue(() =>
                Log.Error(ex, "An unhandled exception has occurred while executing the request."));

            var response = ApiResponse.ErrorResponse("An unexpected error occurred.", context.Response.StatusCode);
            return context.Response.WriteAsJsonAsync(response);
        }
    }
}