using System;
using Fame.Data;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Logging;

namespace Fame.Service.ChangeTracking
{
    public class UnitOfWork : IUnitOfWork
    {
        private IDbContextTransaction _transaction;
        protected FameContext Context;
        private readonly ILogger<UnitOfWork> _logger;
        private bool _disposed;

        public UnitOfWork(FameContext fameContext, ILogger<UnitOfWork> logger)
        {
            Context = fameContext;
            _logger = logger;
        }

        public void Save()
        {
            try
            {
                Context.SaveChanges();
            }
            catch (Exception e)
            {
                _logger.LogError("Database save error");
                throw;
            }
        }

        public void BeginTransaction()
        {
            if (Context.Database.CurrentTransaction == null) _transaction = Context.Database.BeginTransaction();
        }

        public void CommitTransaction()
        {
            try
            {
                _transaction.Commit();
                _transaction.Dispose();
            }
            catch (Exception e)
            {
                _logger.LogError("Commit transaction error");
                throw;
            }
        }

        public void RollBackTransaction()
        {
            _transaction.Rollback();
        }
        
        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _transaction?.Dispose();
                    Context.Dispose();
                }
            }
            _disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}