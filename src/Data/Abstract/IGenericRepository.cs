using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using BlitzFramework.Data.Models;

namespace BlitzFramework.Data.Abstract
{
    public interface IGenericRepository<TEntity, TIdField> where TEntity : Entity<TIdField>, new()
    {
        Task<TEntity> GetAsync(object id);
        Task<IQueryable<TEntity>> GetAllAsync();
        Task<IQueryable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate);
        Task<IQueryable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate, int maxResult);
        Task<TEntity> AddAsync(TEntity entity, object userId);
        Task AddRangeAsync(IEnumerable<TEntity> entities, object userId);
        Task<TEntity> UpdateAsync(TEntity entity, object userId);
        Task RemoveAsync(object id, object userId);
        Task RemoveAsync(TEntity entity, object userId);
        Task RemoveRangeAsync(IEnumerable<TEntity> entities, object userId);
    }
}
