using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SCCB.Core.Infrastructure;
using SCCB.DAL;

namespace SCCB.Repos.Generic
{
    /// <summary>
    /// Generic repository.
    /// </summary>
    /// <typeparam name="TEntity">Type for entity.</typeparam>
    /// <typeparam name="TKey">Type for client.</typeparam>
    public abstract class GenericRepository<TEntity, TKey> : IGenericRepository<TEntity, TKey>
        where TEntity : class, IIdentifiable<TKey>
    {
        /// <summary>
        /// Gets db context instance.
        /// </summary>
        public SCCBDbContext Context { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="GenericRepository{TEntity, TKey}"/> class.
        /// </summary>
        /// <param name="context"> Db context instance.</param>
        public GenericRepository(SCCBDbContext context)
        {
            Context = context;
        }

        /// <inheritdoc />
        public virtual async Task<TKey> AddAsync(TEntity item)
        {
            var entity = await Context.Set<TEntity>()
                .AddAsync(item);

            return entity.Entity.Id;
        }

        /// <inheritdoc />
        public virtual async void AddRangeAsync(IEnumerable<TEntity> items)
        {
            await Context.Set<TEntity>().AddRangeAsync(items);
        }

        /// <inheritdoc />
        public void Update(TEntity item)
        {
            Context.Set<TEntity>().Update(item);
        }

        /// <inheritdoc />
        public virtual void Remove(TEntity item)
        {
            Context.Set<TEntity>()
                .Remove(item);
        }

        /// <inheritdoc />
        public virtual void RemoveRange(IEnumerable<TEntity> items)
        {
            Context.Set<TEntity>()
                .RemoveRange(items);
        }

        /// <inheritdoc />
        public virtual async Task<TEntity> FindAsync(TKey key)
        {
            return await Context.Set<TEntity>()
                .FirstOrDefaultAsync(x => x.Id.Equals(key));
        }

        /// <inheritdoc />
        public virtual async Task<IEnumerable<TEntity>> GetAllAsync()
        {
            return await Context.Set<TEntity>()
                .ToListAsync();
        }

        /// <inheritdoc />
        public async Task<int> CountAsync()
        {
            return await Context.Set<TEntity>()
                .CountAsync();
        }

        /// <inheritdoc />
        public virtual IQueryable<TEntity> GetQuery(bool includeAllNavProp = false)
        {
            var query = Context.Set<TEntity>().AsQueryable();
            if (includeAllNavProp)
            {
                foreach (var property in Context.Model.FindEntityType(typeof(TEntity)).GetNavigations())
                {
                    query = query.Include(property.Name);
                }
            }

            return query;
        }

        /// <inheritdoc />
        public async Task<TEntity> GetAsync(Expression<Func<TEntity, bool>> whereExp)
        {
            var models = await Context.Set<TEntity>().Where(whereExp).SingleOrDefaultAsync();
            return models;
        }
    }
}
