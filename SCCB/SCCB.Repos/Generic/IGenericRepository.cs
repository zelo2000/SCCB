using System.Collections.Generic;
using System.Threading.Tasks;

namespace SCCB.Repos.Generic
{
    public interface IGenericRepository<TEntity, TKey>
    {
        TEntity Add(TEntity item);

        void AddRange(IEnumerable<TEntity> items);

        void Remove(TEntity item);

        void RemoveRange(IEnumerable<TEntity> items);

        Task<TEntity> FindAsync(TKey key);

        Task<IEnumerable<TEntity>> GetAsync();

        Task<int> CountAsync();
    }
}
