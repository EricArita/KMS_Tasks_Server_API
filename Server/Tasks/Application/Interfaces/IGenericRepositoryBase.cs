using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Core.Application.Interfaces
{
    public interface IGenericRepositoryBase<TEntity> where TEntity : class
    {
        IEnumerable<TEntity> Get(Expression<Func<TEntity, bool>> filter = null,
                                 Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderByFunc = null,
                                 string includeProperties = "");
        bool Insert(TEntity entity);
        bool Update(TEntity entityToUpdate);
        void DeleteById(object id);
        void DeleteByObject(TEntity entityToDelete);
    }
}
