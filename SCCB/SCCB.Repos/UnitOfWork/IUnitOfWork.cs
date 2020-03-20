using SCCB.Repos.Users;
using System.Threading.Tasks;

namespace SCCB.Repos.UnitOfWork
{
    public interface IUnitOfWork
    {
        #region Repositories
        public IUserRepository Users { get; }
        #endregion

        /// <summary>
        /// Commit changes asynchronously
        /// </summary>
        Task CommitAsync();

        /// <summary>
        /// Rollback last transaction
        /// </summary>
        void Rollback();
    }
}
