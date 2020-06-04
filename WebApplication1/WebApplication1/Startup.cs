using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using WebApplication1.DAL;
using WebApplication1.EFControllers;
using WebApplication1.Handlers;
using WebApplication1.Middleware;
using WebApplication1.Services;

namespace WebApplication1
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            //HTTP BASIC

            // services.AddAuthentication("AuthenticationBasic")
            //         .AddScheme<AuthenticationSchemeOptions, BasicAuthHandler>("AuthenticationBasic", null);

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                    .AddJwtBearer(options =>
                    {
                        options.TokenValidationParameters = new TokenValidationParameters
                        {
                            ValidateIssuer = true,
                            ValidateAudience = true,
                            ValidateLifetime = true,
                            ValidIssuer = "Gakko",
                            ValidAudience = "Students",
                            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["SecretKey"]))
                        };
                    });

            services.AddScoped<IStudentDbService, SqlServerStudentDbService>();
            services.AddScoped<IStudentDbServ, StudentDbServ>();
            services.AddScoped<EFStudentsController>();



            services.AddControllers()
                    .AddXmlSerializerFormatters();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IStudentDbService service)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();
            
            app.UseMiddleware<LoggingMiddleware>();
            /*
            app.Use(async (contex, next) =>
            {
                if (!contex.Request.Headers.ContainsKey("Index"))
                {
                    contex.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    await contex.Response.WriteAsync("Nie podano indeksu w nag³ówku");
                    return;
                }
                String index = contex.Request.Headers["Index"].ToString();
                var student = service.GetStudent(index);
                if (student.IndexNumber == null)
                {
                    contex.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    await contex.Response.WriteAsync("Brak studenta w bazie");
                    return;
                }
                else
                {
                    await contex.Response.WriteAsync("Student znajduje siê w bazie");
                }

                await next();
            });
            */        

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
