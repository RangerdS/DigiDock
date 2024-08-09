using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DigiDock.Base.Responses
{
    public class ApiResponse
    {
        public int StatusCode { get; set; }
        public bool Success { get; set; }
        public string Message { get; set; }
        public DateTime Timestamp { get; set; }

        public ApiResponse()
        {
            Timestamp = DateTime.UtcNow;
        }

        public ApiResponse(int statusCode, bool success, string message)
        {
            StatusCode = statusCode;
            Success = success;
            Message = message;
            Timestamp = DateTime.UtcNow;
        }

        public static ApiResponse SuccessResponse(string message = "Operation successful")
        {
            return new ApiResponse(200, true, message);
        }

        public static ApiResponse ErrorResponse(string message)
        {
            return new ApiResponse(500, false, message);
        }
    }

    public class ApiResponse<T> : ApiResponse
    {
        public T Data { get; set; }

        public ApiResponse() : base()
        {
        }

        public ApiResponse(int statusCode, bool success, string message, T data) : base(statusCode, success, message)
        {
            Data = data;
        }

        public static new ApiResponse<T> SuccessResponse(T data, string message = "Operation successful")
        {
            return new ApiResponse<T>(200, true, message, data);
        }

        public static new ApiResponse<T> ErrorResponse(string message)
        {
            return new ApiResponse<T>(500, false, message, default);
        }
    }
}
