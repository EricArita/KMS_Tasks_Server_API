﻿using UM.Core.Application.Interfaces;
using UM.Infrastructure.Contexts;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace UM.Infrastructure.Repositories
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        private bool disposed = false;
        private UserManagementDbContext _dbContext;
        private Dictionary<string, dynamic> repositoriesPrototypes;

        public UnitOfWork(UserManagementDbContext dbContext)
        {
            _dbContext = dbContext;
            repositoriesPrototypes = new Dictionary<string, dynamic>();
        }

        public IGenericRepository<T> Repository<T>() where T : class
        {
            var repoType = typeof(T).Name;

            if (!repositoriesPrototypes.ContainsKey(repoType))
            {
                repositoriesPrototypes.Add(repoType, new GenericRepository<T>(_dbContext));
            }

            return (GenericRepository<T>)repositoriesPrototypes[repoType];
        }


        public async Task<int> SaveChangesAsync()
        {
            try
            {
                if (_dbContext != null)
                    return await _dbContext.SaveChangesAsync().ConfigureAwait(false);
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
                //        myString.AppendLine($"--> Property: \"{ve.PropertyName}\", Error: \"{ve.ErrorMessage}\"");
                //    }
                //}
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

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public async Task<IDbContextTransaction> CreateTransaction()
        {
            return await _dbContext.Database.BeginTransactionAsync();
        }

        public EntityEntry<T> Entry<T>(T obj) where T : class
        {
            return _dbContext.Entry(obj);
        }
    }
}
