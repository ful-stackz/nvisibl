using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Nvisibl.DataLibrary.Repositories.Interfaces
{
    public interface IRepositoryT<TEntity> : IRepository
        where TEntity : class
    {
        void Add(TEntity entity);

        Task AddAsync(TEntity entity);

        void AddRange(IEnumerable<TEntity> entities);

        Task AddRangeAsync(IEnumerable<TEntity> entities);

        IEnumerable<TEntity> Find(Expression<Func<TEntity, bool>> predicate);

        TEntity Get(int id);

        Task<TEntity> GetAsync(int id);

        IEnumerable<TEntity> GetRange(int page, int pageSize);

        Task<IEnumerable<TEntity>> GetRangeAsync(int page, int pageSize);

        void Remove(TEntity entity);

        void RemoveRange(IEnumerable<TEntity> entities);

        void Update(TEntity entity);

        void UpdateRange(IEnumerable<TEntity> entities);
    }
}
