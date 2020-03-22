using Microsoft.EntityFrameworkCore;
using SCCB.Core.Infrastructure;
using SCCB.DAL;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SCCB.Repos.Generic
{
    public abstract class GenericRepository<TEntity, TKey> : IGenericRepository<TEntity, TKey>
        where TEntity : class, IIdentifiable<TKey>
    {
        protected readonly SCCBDbContext Context;

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
    }
}
