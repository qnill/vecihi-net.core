using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using vecihi.api.Installers;

namespace vecihi.api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            // Install Service Registrations 
            services.InstallServicesInAssembly(Configuration);

            // Install Autofac
            return AutofacInstaller.Container(services);
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
                app.UseDeveloperExceptionPage();

            // Swagger
            app.UseSwagger();
            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "vecihi.net-core");
                c.RoutePrefix = string.Empty;
            });

            // Authentication
            app.UseAuthentication();

            // Cors
            app.UseCors(builder => builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader().AllowCredentials());

            // Mvc
            app.UseMvc();
        }
    }
}
