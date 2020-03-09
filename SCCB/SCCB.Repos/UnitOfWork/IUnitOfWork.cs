using SCCB.Repos.Users;
using System.Threading.Tasks;

namespace SCCB.Repos.UnitOfWork
{
    public interface IUnitOfWork
    {
        #region Repositories
        public IUserRepository Users { get; }
        #endregion

        Task CommitAsync();

        void Rollback();
    }
}
