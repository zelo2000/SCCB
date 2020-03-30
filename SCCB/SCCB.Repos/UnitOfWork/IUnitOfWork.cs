using SCCB.Repos.Lessons;
using SCCB.Repos.Users;
using SCCB.Repos.Groups;
using SCCB.Repos.Classrooms;
using System.Threading.Tasks;

namespace SCCB.Repos.UnitOfWork
{
    public interface IUnitOfWork
    {
        #region Repositories
        public IUserRepository Users { get; }
        public ILessonRepository Lessons { get; }
        public IGroupRepository Groups { get; }
        public IClassroomRepository Classrooms { get; }
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
