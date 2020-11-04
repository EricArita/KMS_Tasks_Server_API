using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Linq.Expressions;
using Core.Domain.DbEntities;
using Microsoft.EntityFrameworkCore;
using Core.Application.Interfaces;
using NLog;
using Infrastructure.Persistence.Services;

namespace Infrastructure.Persistence.Repositories
{
    public class GenericRepositoryBase<TEntity> : IGenericRepositoryBase<TEntity> where TEntity : class
    {
        internal ApplicationDbContext _dbContext;
        private Logger _logger;

        public GenericRepositoryBase(ApplicationDbContext context)
        {
            _dbContext = context;
            _logger = NLoggerService.GetLogger();
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
    }
}