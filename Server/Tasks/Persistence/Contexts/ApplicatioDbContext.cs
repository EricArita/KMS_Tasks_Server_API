using Application.Interfaces;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Persistence.Contexts
{
    public class ApplicationDbContext : DbContext, IApplicationDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<Example> Examples { get; set; }
        #pragma warning disable CS0114 // Member hides inherited member; missing override keyword
        public async Task<int> SaveChanges()
        {
            return await base.SaveChangesAsync();
        }
    }
}
