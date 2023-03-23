using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UrlShortener.Core.Interfaces;

namespace UrlShortener.Data.Implementations
{
    public class UnitOfWork : IUnitOfWork
    {
        public MyDbContext _dbContext;
        public UnitOfWork(MyDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IGenericRepository<T> GenericRepository<T>() where T : class
        {
            IGenericRepository<T> genericRepository = new GenericRepository<T>(_dbContext);
            return genericRepository;
        }

        public void Save()
        {
            _dbContext.SaveChanges();
        }
        public async Task SaveAsync()
        {
            await _dbContext.SaveChangesAsync();
        }
        private bool disposed = false;
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
