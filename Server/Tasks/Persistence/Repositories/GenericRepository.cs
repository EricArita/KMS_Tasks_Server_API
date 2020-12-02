using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Core.Application.Interfaces;
using NLog;
using Infrastructure.Persistence.Services;
using Infrastructure.Persistence.Contexts;
using Core.Domain.DbEntities;
using System.Threading.Tasks;

namespace Infrastructure.Persistence.Repositories
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class
    {
        internal ApplicationDbContext _dbContext;
        private Logger _logger;

        public GenericRepository(ApplicationDbContext context)
        {
            _dbContext = context;
            _logger = NLoggerService.GetLogger();
        }

        public DbSet<TEntity> GetDbset()
        {
            return _dbContext.Set<TEntity>();
        }

        public virtual IQueryable<TEntity> Get(Expression<Func<TEntity, bool>> filter = null,
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
                return orderByFunc(query);
            }
            else
            {
                return query;
            }
        }

        public virtual TEntity GetByID(object id)
        {
            return GetDbset().Find(id);
        }

        public virtual bool Update(TEntity entityToUpdate)
        {
            try
            {
                GetDbset().Attach(entityToUpdate);
                _dbContext.Entry(entityToUpdate).State = EntityState.Modified;
                return true;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "An error occurred when updating an entity");
                return false;
            }
            finally
            {
                LogManager.Shutdown();
            }
        }

        public virtual bool Insert(TEntity entity)
        {
            try
            {
                GetDbset().Add(entity);
                return true;
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                return false;
            }
            finally
            {
                LogManager.Shutdown();
            }
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

        public virtual async Task<TEntity> InsertAsync(TEntity entity)
        {
            await GetDbset().AddAsync(entity);
            return entity;
        }
    }
}