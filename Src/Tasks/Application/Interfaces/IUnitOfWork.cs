using Microsoft.EntityFrameworkCore.Storage;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Core.Application.Interfaces
{
    public interface IUnitOfWork
    {
        IGenericRepository<T> Repository<T>() where T : class;
        void Dispose();

        //Transactional Methods
        Task<IDbContextTransaction> CreateTransaction();
        Task<int> SaveChangesAsync();
        int SaveChanges();
    }
}
