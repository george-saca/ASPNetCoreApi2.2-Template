using ASP.NET_Core_2._2_WebAPI.Errors;
using ASP.NET_Core_2._2_WebAPI.Exceptions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace ASP.NET_Core_2._2_WebAPI.Middleware
{
    public class HttpStatusCodeExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<HttpStatusCodeExceptionMiddleware> _logger;
        private readonly IErrorBuilder _errorBuilder;

        public HttpStatusCodeExceptionMiddleware(RequestDelegate next, IErrorBuilder errorBuilder, ILoggerFactory loggerFactory)
        {
            _next = next ?? throw new ArgumentNullException(nameof(next));
            _logger = loggerFactory?.CreateLogger<HttpStatusCodeExceptionMiddleware>() ?? throw new ArgumentNullException(nameof(loggerFactory));
            _errorBuilder = errorBuilder;
        }
        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch(HttpStatusCodeException exception)
            {
                ProcessServerError(context, exception, exception.StatusCode);
            }
            catch(Exception exception)
            {
                ProcessServerError(context, exception);
            }
        }

        public async void ProcessServerError(HttpContext context, Exception exception, int statusCode = 500)
        {
            _errorBuilder.Clean();
            _errorBuilder.WithMessage(statusCode, exception.Message, context.Request.Path, exception.Source);
            var errorNode = _errorBuilder.Build();

            context.Response.Clear();
            context.Response.StatusCode = statusCode;

            var headerAccept = context.Request.Headers["Accept"].ToString().ToLower();
            if (headerAccept == "application/xml")
            {
                context.Response.ContentType = "application/problem+xml";
                var xmlResult = SerializeXMLResponse(errorNode);
                await context.Response.WriteAsync(xmlResult, System.Text.Encoding.UTF8);
            }
            else
            {
                context.Response.ContentType = "application/problem+json";
                var jsonResult = JsonConvert.SerializeObject(errorNode);
                await context.Response.WriteAsync(jsonResult, System.Text.Encoding.UTF8);
            }
        }

        // TODO: Move to Serialization dedicated class
        private string SerializeXMLResponse(Error obj)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                XmlSerializer s = new XmlSerializer(typeof(Error));
                Console.WriteLine("Testing for type: {0}", typeof(Error));
                s.Serialize(XmlWriter.Create(stream), obj);
                stream.Flush();
                stream.Seek(0, SeekOrigin.Begin);
                var settingsString = Encoding.UTF8.GetString(stream.ToArray());
                return settingsString;
            }
        }
    }
}
