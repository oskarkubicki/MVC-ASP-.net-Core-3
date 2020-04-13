using apbd3.Services;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace apbd3.Middlewares
{
    public class LoggingMiddleware
    {

        private readonly RequestDelegate _next;

        public LoggingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext httpContext,IStudentsDbService service)
        {
        

            if(httpContext.Request != null)
            
            {
                string path = httpContext.Request.Path; 
                string method = httpContext.Request.Method; 
                string queryString = httpContext.Request.QueryString.ToString();
                string bodyStr = "";

                using (StreamReader reader = new StreamReader(httpContext.Request.Body, Encoding.UTF8, true, 1024, true))
                {
                    bodyStr = await reader.ReadToEndAsync();

                    string log = path + " " + method + " " + queryString + " " + bodyStr+"\n";
              
                     service.SaveLogData(log);
                }

                     if(_next!=null)
                    
                    
                    await _next(httpContext);

            }



        }
            
    }

    }

