using Application.Interfaces;
using Core.Domain.Entities;
using Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Infrastructure.Persistence.Contexts
{
    public class UserManagementDbContext : IdentityDbContext<ApplicationUser>
    {
        public UserManagementDbContext(DbContextOptions<UserManagementDbContext> options) : base(options)
        {
        }

        public async Task<int> SaveChanges()
        {
            return await base.SaveChangesAsync();
        }
    }
}
