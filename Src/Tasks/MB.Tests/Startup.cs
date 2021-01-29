using MB.Core.Application.Interfaces;
using MB.Core.Domain.DbEntities;
using MB.Infrastructure.Contexts;
using MB.Infrastructure.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using NLog.Extensions.Logging;
using NLog.Web;

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

            services.AddLogging(c => c.AddNLog());

            services.AddIdentity<ApplicationUser, IdentityRole>(options => {
                options.Password.RequireUppercase = false;
                options.User.AllowedUserNameCharacters = null;
            }).AddEntityFrameworkStores<ApplicationDbContext>();

            services.AddTransient<IParticipationService, MB.Infrastructure.Services.Internal.ParticipationService>();
        }
    }
}
