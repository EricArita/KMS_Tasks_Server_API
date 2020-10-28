using System.Threading.Tasks;

namespace Core.Application.Interfaces
{
    public interface IUnitOfWork
    {
        Task SaveChangesAsync();
        int SaveChanges();
        void Dispose();
    }
}
