using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Linq.Expressions;
using Core.Domain.DbEntities;
using Microsoft.EntityFrameworkCore;
using Core.Application.Interfaces;

namespace Infrastructure.Persistence.Repositories
{
    public class GenericRepositoryBase<TEntity> : IGenericRepositoryBase<TEntity> where TEntity: class
    {
        internal ApplicationDbContext _dbContext;

        public GenericRepositoryBase(ApplicationDbContext context)
        {
            this._dbContext = context;
        }

        public DbSet<TEntity> GetDbset()
        {
            return _dbContext.Set<TEntity>();
        }

        public virtual IEnumerable<TEntity> Get(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderByFunc = null, string includeProperties = "")
        {
            IQueryable<TEntity> query = GetDbset();

            if (filter != null)
            {

                query = query.Where(filter);
            }

            foreach (var includeProperty in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }

            if (orderByFunc != null)
            {
                return orderByFunc(query).ToList();
            }
            else
            {
                return query.ToList();
            }
        }

        public virtual TEntity GetByID(object id)
        {
            return GetDbset().Find(id);
        }

        public virtual void Update(TEntity entityToUpdate)
        {
            GetDbset().Attach(entityToUpdate);
            _dbContext.Entry(entityToUpdate).State = EntityState.Modified;
        }

        public virtual void Insert(TEntity entity)
        {
            GetDbset().Add(entity);
        }

        public virtual void DeleteById(object id)
        {
            TEntity entityToDelete = GetDbset().Find(id);
            DeleteByObject(entityToDelete);
        }

        public virtual void DeleteByObject(TEntity entityToDelete)
        {
            if (_dbContext.Entry(entityToDelete).State == EntityState.Detached)
            {
                GetDbset().Attach(entityToDelete);
            }
            GetDbset().Remove(entityToDelete);
        }
    }
}