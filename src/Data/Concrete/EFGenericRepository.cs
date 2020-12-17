using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using BlitzFramework.Data.Abstract;
using BlitzFramework.Data.Helpers;
using BlitzFramework.Data.Models;
using BlitzFramework.Extensions;
using BlitzFramework.Resources;
using Microsoft.EntityFrameworkCore;
// ReSharper disable InconsistentNaming

namespace BlitzFramework.Data.Concrete
{
    public class EFGenericRepository<TEntity, TIdField> : IGenericRepository<TEntity, TIdField> where TEntity : Entity<TIdField>, new()
    {
        private readonly DbContext _databaseContext;
        private readonly DbSet<TEntity> _entities;

        protected EFGenericRepository(DbContext context)
        {
            _databaseContext = context;
            _entities = _databaseContext.Set<TEntity>();
        }

        public async Task<TEntity> GetAsync(object id)
        {
            return await _entities.FindAsync(id);
        }

        public virtual async Task<IQueryable<TEntity>> GetAllAsync()
        {
            return await Task.FromResult(_entities.AsNoTracking());
        }

        public async Task<IQueryable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await Task.FromResult(_entities.Where(predicate));
        }

        public async Task<IQueryable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate, int maxResult)
        {
            return await Task.FromResult(_entities.Where(predicate).Take(maxResult));
        }

        public async Task<TEntity> AddAsync(TEntity entity, object userId)
        {
            EFDataContextHelpers.SetAuditProperties(_databaseContext.ChangeTracker, userId);
            var data = await _entities.AddAsync(entity);
            await _databaseContext.SaveChangesAsync();
            return data.Entity;
        }

        public async Task AddRangeAsync(IEnumerable<TEntity> entities, object userId)
        {
            EFDataContextHelpers.SetAuditProperties(_databaseContext.ChangeTracker, userId);
            await _entities.AddRangeAsync(entities);
            await _databaseContext.SaveChangesAsync();
        }

        public async Task<TEntity> UpdateAsync(TEntity entity, object userId)
        {
            EFDataContextHelpers.SetAuditProperties(_databaseContext.ChangeTracker, userId);
            var updateResult = _entities.Update(entity);
            await _databaseContext.SaveChangesAsync();
            return await Task.FromResult(updateResult.Entity);
        }

        public async Task RemoveAsync(object id, object userId)
        {
            var entity = await GetAsync(id);
            if (entity == null)
                throw new ArgumentException(string.Format(Literals.DeleteItemNotFound, id));

            await RemoveAsync(entity, userId);
        }

        public async Task RemoveAsync(TEntity entity, object userId)
        {
            if (entity.HasProperty(Literals.DeletedOnFieldName))
            {
                _entities.Remove(entity);
                await _databaseContext.SaveChangesAsync();
            }
            else
            {
                EFDataContextHelpers.SetDeleteProperties(entity);
                await UpdateAsync(entity, userId);
            }
        }

        public async Task RemoveRangeAsync(IEnumerable<TEntity> entities, object userId)
        {
            foreach (var entity in entities)
            {
                await RemoveAsync(entity, userId);
            }
        }
    }
}
