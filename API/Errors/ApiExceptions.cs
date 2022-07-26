using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Errors
{
    public class ApiExceptions
    {
        public ApiExceptions(int statusCode, string message = null, string details=null)
        {
            StatusCode = statusCode;
            this.message = message;
            this.details = details;
        }

        public int StatusCode { get; set; }
         public string  message { get; set; }
         public string  details { get; set; }
    }
}