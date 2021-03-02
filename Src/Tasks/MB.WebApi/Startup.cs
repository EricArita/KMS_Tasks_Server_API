using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Mvc;
using MB.Infrastructure;
using MB.Core.Application;
using MB.WebApi.Hubs.v1;
using MB.WebApi.Utils;
using MB.Core.Application.Interfaces.Misc;

namespace MB.WebApi
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
            services.AddCors();

            services.AddControllers();

            services.AddApplicationServices();

            services.AddPersistenceServices(Configuration);

            services.AddSingleton<IConnectionManager, ConnectionManager>();

            services.AddOptions();

            #region API Versioning
            services.AddApiVersioning(config =>
            {
                config.DefaultApiVersion = new ApiVersion(1, 0); // Specify the default API Version as 1.0     
                config.AssumeDefaultVersionWhenUnspecified = true; // If the client hasn't specified the API version in the request, use the default API version number               
                config.ReportApiVersions = true; // Advertise the API versions supported for the particular endpoint
            });
            #endregion

            services.AddControllers()
                    .AddNewtonsoftJson(options => options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCors(builder => builder.WithOrigins(new string[] { "http://localhost:8080", "http://localhost:4000", "http://localhost:5002" }).AllowAnyMethod().AllowAnyHeader().AllowCredentials());

            app.UseAuthentication();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHub<GlobalHub>("/signalR");
            });

            #region Swagger
            // Enable middleware to serve generated Swagger as a JSON endpoint.
            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
            // specifying the Swagger JSON endpoint.
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "MainBusinessDoc");
            });
            #endregion
        }
    }
}
