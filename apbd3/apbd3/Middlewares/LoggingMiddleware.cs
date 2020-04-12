using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
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
    }
}
