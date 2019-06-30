using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASP.NET_Core_2._2_WebAPI.Errors
{
    public interface IErrorBuilder
    {
        Error Build();
        void WithMessage(string errorCode, string errorMessage, string errorPath, string errorSource = null);
        void WithMessage(int errorCode, string errorMessage, string errorPath, string errorSource = null);
        void Clean();
    }
}
