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
            this.next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await next(context);
            }
            catch (Exception ex)
            {
                BackgroundJob.Enqueue(() =>
                    Log.Error(ex, "An unhandled exception has occurred while executing the request."));
                await HandleExceptionAsync(context, ex);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            // fill here : add to db error

            var response = new ApiResponse<string>(
                context.Response.StatusCode,
                false,
                "An unexpected error occurred.",
                exception.Message

            );
            // fill here use ApiResponse ??
            return context.Response.WriteAsJsonAsync(response);
        }
    }
}