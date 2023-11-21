using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Errors
{
    public class ApiException : ApiResponse
    {
        public ApiException(int statusCode, string message = null, string stackTraceDetails = null) : base(statusCode, message)
        {
            Details = stackTraceDetails;
        }

        public string Details { get; set; }
    }
}