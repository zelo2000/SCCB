using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace SCCB.Repos.Generic
{
    /// <summary>
    /// Interface for base repository.
    /// </summary>
    /// <typeparam name="TEntity">Generic entity parameter.</typeparam>
    /// <typeparam name="TKey">Generic key parameter.</typeparam>
    public interface IGenericRepository<TEntity, TKey>
    {
        /// <summary>
        /// Add entity.
        /// </summary>
        /// <param name="item">Entity.</param>
        /// <returns>Entity id.</returns>
        Task<TKey> AddAsync(TEntity item);

        /// <summary>
        /// Add list of entities.
        /// </summary>
        /// <param name="items">List of entities.</param>
        void AddRangeAsync(IEnumerable<TEntity> items);

        /// <summary>
        /// Update entity.
        /// </summary>
        /// <param name="item">Entity.</param>
        void Update(TEntity item);

        /// <summary>
        /// Delete entity.
        /// </summary>
        /// <param name="item">Entity.</param>
        void Remove(TEntity item);

        /// <summary>
        /// Delete list of entities.
        /// </summary>
        /// <param name="items">List of entities.</param>
        void RemoveRange(IEnumerable<TEntity> items);

        /// <summary>
        /// Find an entity by primary key.
        /// </summary>
        /// <param name="key">Key.</param>
        /// <returns>Entity.</returns>
        Task<TEntity> FindAsync(TKey key);

        /// <summary>
        /// Get list of entities.
        /// </summary>
        /// <returns>List of entities.</returns>
        Task<IEnumerable<TEntity>> GetAllAsync();

        /// <summary>
        /// Get count of entities.
        /// </summary>
        /// <returns>Count.</returns>
        Task<int> CountAsync();

        /// <summary>
        /// Gets entity async.
        /// </summary>
        /// <param name="whereExp">Where expresion.</param>
        /// <returns>Entity instance.</returns>
        Task<TEntity> GetAsync(Expression<Func<TEntity, bool>> whereExp);

        /// <summary>
        /// Queries the given include all navigation properties.
        /// </summary>
        /// <param name="includeAllNavProp">(Optional) True to include, false to exclude all navigation property.</param>
        /// <returns>An IQueryable&lt;TEntity&gt;.</returns>
        IQueryable<TEntity> GetQuery(bool includeAllNavProp = false);
    }
}
