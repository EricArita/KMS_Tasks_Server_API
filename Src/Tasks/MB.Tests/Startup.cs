using MB.Core.Application.Interfaces;
using MB.Core.Domain.DbEntities;
using MB.Infrastructure.Contexts;
using MB.Infrastructure.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using NLog;
using NLog.Config;
using NLog.Extensions.Logging;
using NLog.Targets;
using NLog.Web;
using Xunit.Abstractions;
using Microsoft.Extensions.Logging;
using System;
using Xunit.DependencyInjection.Logging;
using Xunit.DependencyInjection;

namespace MB.Tests
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
               options.UseSqlServer("Data Source=.\\MSSQL15;Initial Catalog=KMS_Tasks;Integrated Security=True"));

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

            services.AddTransient<IParticipationService, Infrastructure.Services.Internal.ParticipationService>();
        }

        // Uncomment for debugging
        //public void Configure(IServiceProvider provider)
        //{
        //    provider.GetRequiredService<ILoggerFactory>().AddProvider(new XunitTestOutputLoggerProvider(provider.GetRequiredService<ITestOutputHelperAccessor>()));
        //}
    }
}
