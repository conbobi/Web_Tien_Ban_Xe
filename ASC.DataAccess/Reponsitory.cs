using ASC.DataAccess.Interfaces;
using ASC.Model.Basetypes;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASC.DataAccess
{
    public class Repository<T> : IRepository<T> where T : BaseEntity, new()
    {
        private readonly DbContext _dbContext;

        public Repository(DbContext dbContext)
        {
            this._dbContext = dbContext;
        }

        public async Task<T> AddAsync(T entity)
        {
            var entityToInsert = entity as BaseEntity;
            entityToInsert.CreatedDate = DateTime.UtcNow;
            entityToInsert.UpdatedDate = DateTime.UtcNow;
            var result = await _dbContext.Set<T>().AddAsync(entity);
            return result.Entity;
        }

        public void Update(T entity)
        {
            var entityToUpdate = entity as BaseEntity;
            entityToUpdate.UpdatedDate = DateTime.UtcNow;
            _dbContext.Set<T>().Update(entity);
        }

        public void Delete(T entity)
        {
            var entityToDelete = entity as BaseEntity;
            entityToDelete.UpdatedDate = DateTime.UtcNow;
            entityToDelete.IsDeleted = true;
            _dbContext.Set<T>().Remove(entity);
        }

        public async Task<T> FindAsync(string partitionKey, string rowKey)
        {
            var result = await _dbContext.Set<T>().FindAsync(partitionKey, rowKey);
            return result;
        }

        public async Task<IEnumerable<T>> FindAllByPartitionKeyAsync(string partitionKey)
        {
            var result =await _dbContext.Set<T>().Where(t => t.PartitionKey == partitionKey).ToListAsync();
            return result;
        }

        public async Task<IEnumerable<T>> FindAllAsync()
        {
            var result = await _dbContext.Set<T>().ToListAsync();
            return result;
        }
    }
}
