using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EmailService.MiddleWares;
using EmailService.Services;
using EmailService.Services.Interfaces;
using EmailService.Swagger;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.Swagger;

namespace EmailService
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
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            services.AddSingleton<IConfiguration>(Configuration);
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "Authentication API", Version = "v1" });
                c.DocumentFilter<DocumentFilter>();

            });

            services.AddTransient<IEmailService, MailService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {


            app.UseSwagger();

            app.Use(async (context, next) =>
            {
                var request = context.Request;
                if (request.Path == "/swagger" && !request.IsHttps)
                    context.Response.Redirect("https://" + request.Host + request.Path);
                else
                    await next();
            });

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Email API V1");
            });



            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();

            }
            app.UseRewriter(new RewriteOptions().AddRedirectToHttpsPermanent());

            app.UseExceptionHandler(new ExceptionHandlerOptions
            {
                ExceptionHandler = new JsonExceptionMiddleware().Invoke
            });

            app.UseMvc();
            app.UseStaticFiles();
        }
    }
}
