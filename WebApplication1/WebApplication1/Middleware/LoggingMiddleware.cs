﻿using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApplication1.Middleware
{
    public class LoggingMiddleware
    {

        private readonly RequestDelegate _next;

        public LoggingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            context.Request.EnableBuffering();
            if (context.Request != null)
            {
                string path = context.Request.Path; 
                string method = context.Request.Method; 
                string queryString = context.Request.QueryString.ToString(); 
                string bodyStr = "";

                using (var reader = new StreamReader(context.Request.Body, Encoding.UTF8, true, 1024, true))
                {
                    bodyStr = await reader.ReadToEndAsync();
                    context.Request.Body.Seek(0, SeekOrigin.Begin);
                }
                string[] lines = {path, method, queryString, bodyStr };
                System.IO.File.WriteAllLines(@"C:\Users\fongo\Desktop\CW4\WebApplication1\requestsLog.txt", lines);

               
            }

            await _next(context);
        }
    }
}
