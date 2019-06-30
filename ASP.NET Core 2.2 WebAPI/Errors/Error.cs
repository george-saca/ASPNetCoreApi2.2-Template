using System;
using System.Collections.Generic;

namespace ASP.NET_Core_2._2_WebAPI.Errors
{
    [Serializable]
    public class Error
    {
        private List<ErrorResult> _errorResults = new List<ErrorResult>();
        public List<ErrorResult> ErrorResults => _errorResults;

        public void AddError(string errorCode, string errorMessage, string errorPath, string errorSource = null)
        {
            _errorResults.Add(new ErrorResult(errorCode, errorMessage, errorPath, errorSource));
        }
    }
}
