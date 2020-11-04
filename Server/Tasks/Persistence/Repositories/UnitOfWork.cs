using Core.Application.Interfaces;
using Core.Domain.DbEntities;
using System;
using System.Data.Entity.Validation;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Persistence.Repositories
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        private bool disposed = false;
        private ApplicationDbContext _dbContext;
        private TaskRepository taskRepository;

        public UnitOfWork()
        {
            _dbContext = new ApplicationDbContext();
        }

        public TaskRepository TaskRepository
        {
            get
            {
                if (taskRepository == null)
                {
                    taskRepository = new TaskRepository(_dbContext);
                }
                return taskRepository;
            }
        }

        public async Task SaveChangesAsync()
        {
            try
            {
                if (_dbContext != null)
                    await _dbContext.SaveChangesAsync().ConfigureAwait(false);
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
