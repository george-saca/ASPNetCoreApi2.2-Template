using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ASP.NET_Core_2._2_WebAPI.Errors;
using ASP.NET_Core_2._2_WebAPI.Middleware;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Swashbuckle.AspNetCore.Swagger;

namespace ASP.NET_Core_2._2_WebAPI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc(o =>
            {
                o.RespectBrowserAcceptHeader = true;
                o.OutputFormatters.Add(new XmlSerializerOutputFormatter());
                o.ReturnHttpNotAcceptable = true;
                o.Filters.Add(new ProducesAttribute("application/json", "application/xml", "application/text"));
            }        
            ).SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "My API", Version = "v1" });
            });

            services.AddTransient<IErrorBuilder, ErrorBuilder>();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IErrorBuilder errorBuilder)
        {
            if (env.IsDevelopment())
            {
                //app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpStatusCodeExceptionMiddleware();

            //HandleExceptions(app);

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            });

            app.UseHttpsRedirection();
            app.UseMvc();
        }

        //private void HandleExceptions(IApplicationBuilder app, IErrorBuilder errorBuilder)
        //{
        //    app.UseExceptionHandler(appBuilder =>
        //    {
        //        appBuilder.Run(async context =>
        //        {
        //            var exceptionHandlerFeature = context.Features.Get<IExceptionHandlerPathFeature>();
        //            if (exceptionHandlerFeature != null)
        //            {
        //                errorBuilder.Clean();
        //                errorBuilder.WithMessage(500, exceptionHandlerFeature.Error.Message, exceptionHandlerFeature.Path, exceptionHandlerFeature.Error.Source);
        //                var errorNode = errorBuilder.Build();
        //                context.Response.StatusCode = 500;
        //                context.Response.ContentType = "application/problem+json";
        //                var jsonResult = JsonConvert.SerializeObject(errorNode);

        //                // logging

        //                await context.Response.WriteAsync(jsonResult, System.Text.Encoding.UTF8);
        //            }
        //        });
        //    });
        //}

        private void HandleExceptions(IApplicationBuilder app/*, ILoggingService loggingService*/)
        {
            app.UseExceptionHandler(appBuilder =>
            {
                appBuilder.Run(async context =>
                {
                    var exceptionHandlerFeature = context.Features.Get<IExceptionHandlerFeature>();
                    if (exceptionHandlerFeature != null)
                    {
                        //loggingService.Log(GetType(), Logging.LogLevel.Error, LogMessages.GlobalError, exceptionHandlerFeature.Error.Message, LogCodes.InternalServerError500HttpStatusCode, exceptionHandlerFeature.Error);
                    }
                    context.Response.StatusCode = 500;
                    await context.Response.WriteAsync("An unexpected fault happened. Try again later.");
                });
            });
        }
    }
}
