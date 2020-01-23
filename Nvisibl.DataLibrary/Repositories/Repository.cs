using Nvisibl.DataLibrary.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Nvisibl.DataLibrary.Repositories
{
    internal abstract class Repository<TEntity> : IRepository<TEntity>
        where TEntity : class
    {
        public Repository(DbContext context)
        {
            Context = context ?? throw new ArgumentNullException(nameof(context));
        }

        protected DbContext Context { get; }

        public virtual void Add(TEntity entity)
        {
            if (entity is null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            Context.Set<TEntity>().Add(entity);
        }

        public virtual async Task AddAsync(TEntity entity)
        {
            if (entity is null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            await Context.Set<TEntity>().AddAsync(entity);
        }

        public virtual void AddRange(IEnumerable<TEntity> entities)
        {
            if (entities is null)
            {
                throw new ArgumentNullException(nameof(entities));
            }

            Context.Set<TEntity>().AddRange(entities);
        }

        public virtual async Task AddRangeAsync(IEnumerable<TEntity> entities)
        {
            if (entities is null)
            {
                throw new ArgumentNullException(nameof(entities));
            }

            await Context.Set<TEntity>().AddRangeAsync(entities);
        }

        public virtual IEnumerable<TEntity> Find(Expression<Func<TEntity, bool>> predicate)
        {
            if (predicate is null)
            {
                throw new ArgumentNullException(nameof(predicate));
            }

            return Context.Set<TEntity>().Where(predicate);
        }

        public virtual TEntity Get(int id)
        {
            return Context.Set<TEntity>().Find(id);
        }

        public virtual async Task<TEntity> GetAsync(int id)
        {
            return await Context.Set<TEntity>().FindAsync(id);
        }

        public virtual IEnumerable<TEntity> GetRange(int page, int pageSize)
        {
            return Context.Set<TEntity>()
                .Skip((page < 0 ? 0 : page) * pageSize)
                .Take(pageSize)
                .ToList();
        }

        public virtual async Task<IEnumerable<TEntity>> GetRangeAsync(int page, int pageSize)
        {
            return await Context.Set<TEntity>()
                .Skip((page < 0 ? 0 : page) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public virtual void Remove(TEntity entity)
        {
            if (entity is null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            Context.Set<TEntity>().Remove(entity);
        }

        public virtual void RemoveRange(IEnumerable<TEntity> entities)
        {
            if (entities is null)
            {
                throw new ArgumentNullException(nameof(entities));
            }

            Context.Set<TEntity>().RemoveRange(entities);
        }

        public virtual void Update(TEntity entity)
        {
            if (entity is null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            Context.Set<TEntity>().Update(entity);
        }

        public virtual void UpdateRange(IEnumerable<TEntity> entities)
        {
            if (entities is null)
            {
                throw new ArgumentNullException(nameof(entities));
            }

            Context.Set<TEntity>().UpdateRange(entities);
        }
    }
}
