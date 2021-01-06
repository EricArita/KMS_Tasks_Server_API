using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using UM.Infrastructure.Contexts;
using Microsoft.Extensions.DependencyInjection;
using UM.Infrastructure.Repositories;
using UM.Core.Application.Interfaces;
using Microsoft.OpenApi.Models;
using UM.Infrastructure.Misc;

namespace UM.Infrastructure
{
    public static class ServiceRegister
    {
        /// <summary>
        /// Register all dependencies and services of Infrastructure layer. This method makes solution get the maintainability and
        /// can be used in every layer of solution
        /// </summary>
        /// <param name="services"></param>
        public static void AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<UserManagementDbContext>(options =>
               options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"), b => b.MigrationsAssembly(typeof(UserManagementDbContext).Assembly.FullName)));
           
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "UserManagementApiDoc",
                });
                c.AddSecurityDefinition("bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Description = "`Token without Bearer prefix plz` - without `Bearer_` prefix",
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header
                });
                c.OperationFilter<AuthorizationHeader_Param_OperationFilter>();
            });
        }
    }
}
