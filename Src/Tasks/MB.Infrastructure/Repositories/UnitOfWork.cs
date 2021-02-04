using MB.Core.Application.Interfaces;
using MB.Infrastructure.Contexts;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MB.Infrastructure.Repositories
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        private bool disposed = false;
        private ApplicationDbContext _dbContext;
        private Dictionary<string, IRepository> repositoriesPrototypes;

        public UnitOfWork(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
            repositoriesPrototypes = new Dictionary<string, IRepository>();
        }

        public virtual IGenericRepository<T> Repository<T>() where T : class
        {
            var repoType = typeof(T).Name;

            if (!repositoriesPrototypes.ContainsKey(repoType))
            {
                repositoriesPrototypes.Add(repoType, new GenericRepository<T>(_dbContext));
            }

            return (GenericRepository<T>)repositoriesPrototypes[repoType];
        }


        public virtual async Task<int> SaveChangesAsync()
        {
            try
            {
                if (_dbContext != null)
                    return await _dbContext.SaveChangesAsync().ConfigureAwait(false);
            }
            catch (Exception e)
            {
                StringBuilder myString = new StringBuilder("EF Core received an error:");
                myString.Append(e.ToString());
                //foreach (var eve in e.EntityValidationErrors)
                //{
                //    myString.AppendLine($"Entity of type \"{eve.Entry.Entity.GetType().Name}\" in state \"{eve.Entry.State}\" has the following validation errors:");
                //    foreach (var ve in eve.ValidationErrors)
                //    {
                //        myString.AppendLine($"--> Property: \"{ve.PropertyName}\", Error: \"{ve.ErrorMessage}\"");
                //    }
                //}
                throw new Exception(myString.ToString());
            }
            return 0;
        }
        
        public virtual int SaveChanges()
        {
            try
            {
                if (_dbContext != null)
                    return _dbContext.SaveChanges();
            }
            catch (Exception e)
            {
                StringBuilder myString = new StringBuilder("EF Core received an error:");
                myString.Append(e);
                //foreach (var eve in e.EntityValidationErrors)
                //{
                //    myString.AppendLine($"Entity of type \"{eve.Entry.Entity.GetType().Name}\" in state \"{eve.Entry.State}\" has the following validation errors:");
                //    foreach (var ve in eve.ValidationErrors)
                //    {
                //        myString.AppendLine($"- Property: \"{ve.PropertyName}\", Error: \"{ve.ErrorMessage}\"");
                //    }
                //}
                throw new Exception(myString.ToString());
            }
            return 0;
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    _dbContext.Dispose();
                }
            }
            this.disposed = true;
        }

        public virtual void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public virtual async Task<IDbContextTransaction> CreateTransaction()
        {
            return await _dbContext.Database.BeginTransactionAsync();
        }

        public virtual EntityEntry<T> Entry<T>(T obj) where T : class
        {
            return _dbContext.Entry(obj);
        }
    }
}
