using DataAcessLayer.Interfaces;
using DataAcessLayer.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DataAcessLayer
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class
    {
        public readonly DbSet<TEntity> _dbSet;
        public readonly GroceryCloudContext _dbContext;
        public GenericRepository(GroceryCloudContext dbContext)
        {
            _dbSet = dbContext.Set<TEntity>();
            _dbContext = dbContext;
        }

        public DbSet<TEntity> Get()
        {
            return _dbSet;
        }

        public async Task<TEntity> GetById(int id)
        {
            var data = await _dbSet.FindAsync(id);
            return data;
        }

        public async Task Add(TEntity entity)
        {
            await _dbSet.AddAsync(entity);
        }

        public void Update(TEntity entity)
        {
            //_dbSet.Attach(entity);

            //foreach (PropertyInfo prop in entity.GetType().GetProperties())
            //{
            //    if (prop.GetGetMethod().IsVirtual) continue;
            //    if (prop.Name == "Id") continue;
            //    if (prop.GetValue(entity, null) != null)
            //    {
            //        _dbContext.Entry(entity).Property(prop.Name).IsModified = true;
            //    }

            //}
            _dbSet.Attach(entity);
            _dbContext.Entry(entity).State = EntityState.Modified;
        }

        public async Task Delete(int id)
        {
            var entity = await GetById(id);
            _dbSet.Remove(entity);
        }
        public async Task DeleteComplex(object firstKey, object secondKey)
        {
            var entity = await _dbSet.FindAsync(firstKey,secondKey);
            _dbSet.Remove(entity);
        }
    }
}
