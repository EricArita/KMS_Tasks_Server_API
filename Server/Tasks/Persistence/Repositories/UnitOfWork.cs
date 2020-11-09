using Core.Application.Interfaces;
using Infrastructure.Persistence.Contexts;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Persistence.Repositories
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        private bool disposed = false;
        private ApplicationDbContext _dbContext;
        private Dictionary<string, dynamic> repositoriesPrototypes;

        public UnitOfWork(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
            repositoriesPrototypes = new Dictionary<string, dynamic>();
        }

        public T Repository<T>() where T : class
        {
            var repoType = typeof(T).Name;

            if (!repositoriesPrototypes.ContainsKey(repoType))
            {
                switch (repoType)
                {
                    case "TaskRepository":
                        repositoriesPrototypes.Add(repoType, new TaskRepository(_dbContext));
                        break;
                    default:
                        return null;
                }
            }

            return (T)repositoriesPrototypes[repoType];
        }


        public async Task<int> SaveChangesAsync()
        {
            try
            {
                if (_dbContext != null)
                    return await _dbContext.SaveChangesAsync().ConfigureAwait(false);
            }
            catch (DbEntityValidationException e)
            {
                StringBuilder myString = new StringBuilder("EF Core received an error:");
                foreach (var eve in e.EntityValidationErrors)
                {
                    myString.AppendLine($"Entity of type \"{eve.Entry.Entity.GetType().Name}\" in state \"{eve.Entry.State}\" has the following validation errors:");
                    foreach (var ve in eve.ValidationErrors)
                    {
                        myString.AppendLine($"--> Property: \"{ve.PropertyName}\", Error: \"{ve.ErrorMessage}\"");
                    }
                }
                throw new Exception(myString.ToString());
            }
            return 0;
        }
        
        public int SaveChanges()
        {
            try
            {
                if (_dbContext != null)
                    return _dbContext.SaveChanges();
            }
            catch (DbEntityValidationException e)
            {
                StringBuilder myString = new StringBuilder("EF Core received an error:");
                foreach (var eve in e.EntityValidationErrors)
                {
                    myString.AppendLine($"Entity of type \"{eve.Entry.Entity.GetType().Name}\" in state \"{eve.Entry.State}\" has the following validation errors:");
                    foreach (var ve in eve.ValidationErrors)
                    {
                        myString.AppendLine($"- Property: \"{ve.PropertyName}\", Error: \"{ve.ErrorMessage}\"");
                    }
                }
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

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
