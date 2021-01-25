using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace UM.Core.Application.Interfaces
{
    public interface IGenericRepository<TEntity> where TEntity : class
    {
        DbSet<TEntity> GetDbset();
        IQueryable<TEntity> Get(Expression<Func<TEntity, bool>> filter = null,
                                 Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderByFunc = null,
                                 string includeProperties = "");
        bool Insert(TEntity entity);
        bool Update(TEntity entityToUpdate);
        void DeleteById(object id);
        void DeleteByObject(TEntity entityToDelete);
        Task<TEntity> InsertAsync(TEntity entity);
    }
}
