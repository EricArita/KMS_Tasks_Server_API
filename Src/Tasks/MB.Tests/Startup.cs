using MB.Core.Application.Interfaces;
using MB.Core.Domain.DbEntities;
using MB.Infrastructure.Contexts;
using MB.Infrastructure.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using NLog.Extensions.Logging;
using NLog.Web;
using Microsoft.Extensions.Logging;
using System;
using Xunit.DependencyInjection.Logging;
using Xunit.DependencyInjection;
using Microsoft.Extensions.Configuration;

namespace MB.Tests
{
    public class Startup
    {
        public static IConfiguration InitConfiguration()
        {
            var config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();
            return config;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            var configuration = InitConfiguration();

            services.AddDbContext<ApplicationDbContext>(options =>
               options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

            #region Unit of work
            services.AddTransient<IUnitOfWork, UnitOfWork>();
            #endregion

            services.AddLogging(c => {
                c.AddNLog();               
            });

            services.AddIdentity<ApplicationUser, IdentityRole>(options => {
                options.Password.RequireUppercase = false;
                options.User.AllowedUserNameCharacters = null;
            }).AddEntityFrameworkStores<ApplicationDbContext>();
        }
       
        public void Configure(IServiceProvider provider)
        {
            // Uncomment for debugging
            //provider.GetRequiredService<ILoggerFactory>().AddProvider(new XunitTestOutputLoggerProvider(provider.GetRequiredService<ITestOutputHelperAccessor>(), (str, logLevel) => {
            //    return logLevel >= LogLevel.Warning;
            //}));
        }
    }
}
