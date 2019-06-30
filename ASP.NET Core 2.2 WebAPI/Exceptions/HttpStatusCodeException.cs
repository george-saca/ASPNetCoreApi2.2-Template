using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASP.NET_Core_2._2_WebAPI.Exceptions
{
    public class HttpStatusCodeException : Exception
    {
        public int StatusCode { get; set; }
        public string ContentType { get; set; } = @"text/plain";

        public HttpStatusCodeException(int statusCode)
        {
            this.StatusCode = statusCode;
        }

        public HttpStatusCodeException(int statusCode, string contentType, string message) : base(message)
        {
            this.StatusCode = statusCode;
            this.ContentType = contentType;
        }

        //public HttpStatusCodeException(int statusCode, Exception inner) : this(statusCode, inner.ToString()) { }

        //public HttpStatusCodeException(int statusCode,  errorObject) : this(statusCode, errorObject.ToString())
        //{
        //    this.ContentType = @"application/json";
        //}
    }
}
