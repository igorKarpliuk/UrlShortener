using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using UrlShortener.Core.Interfaces;

namespace UrlShortener.Data.Implementations
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class
    {
        private readonly MyDbContext _dbContext;
        DbSet<TEntity> _DbSet;
        public GenericRepository(MyDbContext context)
        {
            _dbContext = context;
            _DbSet = context.Set<TEntity>();
        }
        public void Add(TEntity entity)
        {
            _DbSet.Add(entity);
            _dbContext.SaveChanges();
        }
        public void Update(TEntity entity)
        {
            _dbContext.Entry(entity).State = EntityState.Modified;
            _dbContext.SaveChanges();
        }
        public void Delete(TEntity entity)
        {
            _DbSet.Remove(entity);
            _dbContext.SaveChanges();
        }
        public IEnumerable<TEntity> GetAll()
        {
            return _DbSet.AsNoTracking();
        }
        public IEnumerable<TEntity> Get(Func<TEntity, bool> predicate)
        {
            return _DbSet.AsNoTracking().Where(predicate);
        }
        public IEnumerable<TEntity> GetWithInclude(params Expression<Func<TEntity, object>>[] includeProperties)
        {
            return Include(includeProperties);
        }
        public IEnumerable<TEntity> GetWithInclude(Func<TEntity, bool> predicate,
            params Expression<Func<TEntity, object>>[] includeProperties)
        {
            var query = Include(includeProperties);
            return query.Where(predicate);
        }
        private IQueryable<TEntity> Include(params Expression<Func<TEntity, object>>[] includeProperties)
        {
            IQueryable<TEntity> query = _DbSet.AsNoTracking();
            return includeProperties
                .Aggregate(query, (current, includeProperty) => current.Include(includeProperty));
        }
    }
}
