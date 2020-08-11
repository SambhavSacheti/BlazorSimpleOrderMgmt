﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.OData.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Net.Http.Headers;

namespace ODataWithoutEF
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
            services.AddMvc(MvcOptions=>MvcOptions.EnableEndpointRouting = false).SetCompatibilityVersion(CompatibilityVersion.Latest);
            services.AddOData();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.EnvironmentName == "Development")
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseCors(policy => 
            policy.WithOrigins("http://localhost:5000", "https://localhost:5001")
            .AllowAnyMethod()
             .WithHeaders(HeaderNames.ContentType, HeaderNames.Authorization)
             .AllowCredentials());

            app.UseHttpsRedirection();
            app.UseMvc(builder =>
            {
                builder.EnableDependencyInjection();
                builder.Expand().Select().OrderBy().Count().Filter().SkipToken().MaxTop(5000);
            });
        }
    }
}