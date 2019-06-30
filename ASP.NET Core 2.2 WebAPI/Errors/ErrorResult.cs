using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASP.NET_Core_2._2_WebAPI.Errors
{
    [Serializable]
    public class ErrorResult
    {
        public string ErrorCode { get; set; }
        public string ErrorMessage { get; set; }
        public string ErrorPath { get; set; }
        public string ErrorSource { get; set; }

        public ErrorResult()
        {
        }

        public ErrorResult(string code, string message, string path, string source = null)
        {
            ErrorCode = code;
            ErrorMessage = message;
            ErrorPath = path;
            ErrorSource = source;
        }
    }
}
