using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IT.BFF.Domain.Core;
using IT.BFF.Infra.TelegraphConnector;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;

namespace IT.BFF.WebApi
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
            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "IT.BFF.WebApi",
                    Description = "A simple BackForFront implementation to run " +
                                  "between our backend core services and the Android Client.",
                    Contact = new OpenApiContact
                    {
                        Name = "Sergio Castell (linuxct)",
                        Email = "linuxct@linuxct.space",
                        Url = new Uri("https://twitter.com/linuxct")
                    }
                });
            });

            services.AddScoped<ICoreService, CoreService>();
            services.AddScoped<ITelegraphConnector, TelegraphConnector>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger();
            
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "IT.BFF.WebApi v1");
                c.RoutePrefix = string.Empty;
            });
            
            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}