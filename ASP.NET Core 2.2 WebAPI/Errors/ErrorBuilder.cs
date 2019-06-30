using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASP.NET_Core_2._2_WebAPI.Errors
{
    public class ErrorBuilder : IErrorBuilder
    {
        private List<Tuple<string, string, string, string>> _errors = new List<Tuple<string, string, string, string>>();

        public void WithMessage(string errorCode, string errorMessage, string errorPath, string errorSource = null)
        {
            _errors.Add(new Tuple<string, string, string, string>(errorCode, errorMessage, errorPath, errorSource));
        }

        public void WithMessage(int errorCode, string errorMessage, string errorPath, string errorSource = null)
        {
            WithMessage(errorCode.ToString(), errorMessage, errorPath, errorSource);
        }

        public Error Build()
        {
            var error = new Error();
            foreach (var item in _errors)
            {
                error.AddError(item.Item1, item.Item2, item.Item3, item.Item4);
            }
            return error;
        }

        public void Clean()
        {
            _errors = new List<Tuple<string, string, string, string>>();
        }
    }
}
