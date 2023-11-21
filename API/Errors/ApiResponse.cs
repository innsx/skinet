using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Errors
{
    public class ApiResponse
    {
        public int StatusCode { get; set; }
        public object Message { get; set; }

        public ApiResponse(int statusCode, string message = null)
        {
            StatusCode = statusCode;
            Message = message ?? GetDefaultMessageForStatusCode(statusCode);
        }

        private string GetDefaultMessageForStatusCode(int statusCode)
        {
            return statusCode switch // C# 8.0 SWITCH STATMENT
            {
                400 => "Client-side Status Code 400 Error: You have made A BAD REQUEST!",
                401 => "Client-side Status Code 401 Error: You are UNAUTHORIZED to make this REQUEST!",
                403 => "Client-side Status Code 403 Error: You are FORBIDDEN to make this REQUEST!",
                404 => "Client-side Status Code 404 Error: The Resources you've requested is NOT found!",
                500 => "Server-side Status Code 500 Error: You've made an Internal Server Error!",
                501 => "Server-side Status Code 501 Error: Not implemented!",
                502 => "Server-side Status Code 502 Error: Bad Gateway Proxy Error!",
                503 => "Server-side Status Code 503 Error: Services is Unavailable!",
                504 => "Server-side Status Code 504 Error: Gateway timeout!",
                505 => "Server-side Status Code 505 Error: HTTP Version Not Supported!",

                _ => null
            };
        }
    }
}