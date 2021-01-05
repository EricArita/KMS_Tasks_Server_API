using Core.Application.Interfaces;
using Core.Application.Models;
using Core.Domain.DbEntities;
using Infrastructure.Persistence.Contexts;
using Infrastructure.Persistence.Misc;
using Infrastructure.Persistence.Services;
using Infrastructure.Persistence.SettingModels;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System;
using System.Text;

namespace Persistence
{
    public static class ServiceRegister
    {
        /// <summary>
        /// Register all dependencies and services of Persistence layer. This method makes solution get the maintainability and
        /// can be used in every layer of solution
        /// </summary>
        /// <param name="services"></param>
        public static void AddPersistenceServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<JWT>(configuration.GetSection("Authentication:JWT"));
            services.Configure<FacebookAuthSettings>(configuration.GetSection("Authentication:Facebook"));

            services.AddDbContext<ApplicationDbContext>(options =>
               options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"), b => b.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)));

            #region Identity framework
            services.AddIdentity<ApplicationUser, IdentityRole>(options => {
                options.Password.RequireUppercase = false;
                options.User.AllowedUserNameCharacters = null;
            }).AddEntityFrameworkStores<ApplicationDbContext>();
            #endregion

            #region Authentication
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(o =>
            {
                o.RequireHttpsMetadata = false;
                o.SaveToken = false;
                o.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero,
                    ValidIssuer = configuration["Authentication:JWT:Issuer"],
                    ValidAudience = configuration["Authentication:JWT:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Authentication:JWT:Key"]))
                };
            })
            .AddFacebook(facebookOptions =>
            {
                facebookOptions.AppId = configuration["Authentication:Facebook:AppId"];
                facebookOptions.AppSecret = configuration["Authentication:Facebook:AppSecret"];
            });
            #endregion

            #region Swagger
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "TasksApiDoc",
                });
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Description = "`Token without Bearer prefix plz` (type in without `Bearer_` prefix)",
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header
                });
                c.OperationFilter<AuthorizationHeader_Param_OperationFilter>();
                c.OperationFilter<DefaultForMostRequests_OperationFilter>();
            });
            #endregion

            #region Add scoped services
            services.AddScoped<IAuthentication, AuthenticationService>();
            services.AddScoped<ITaskService, TaskService>();
            services.AddScoped<IProjectService, ProjectService>();
            services.AddScoped<IParticipationService, ParticipationService>();
            services.AddScoped<IUserService, UserService>();
            #endregion
        }
    }
}
