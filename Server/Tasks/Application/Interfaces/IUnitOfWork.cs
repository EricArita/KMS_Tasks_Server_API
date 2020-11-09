using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Core.Application.Interfaces
{
    public interface IUnitOfWork
    {
        T Repository<T>() where T : class;
        Task<int> SaveChangesAsync();
        int SaveChanges();
        void Dispose();
    }
}
