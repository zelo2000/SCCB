using System.Threading.Tasks;
using SCCB.Repos.Classrooms;
using SCCB.Repos.Groups;
using SCCB.Repos.Lectors;
using SCCB.Repos.Lessons;
using SCCB.Repos.Users;

namespace SCCB.Repos.UnitOfWork
{
    public interface IUnitOfWork
    {
        #region Repositories
        public IUserRepository Users { get; }

        public ILessonRepository Lessons { get; }

        public IGroupRepository Groups { get; }

        public IClassroomRepository Classrooms { get; }

        public ILectorRepository Lectors { get; }
        #endregion

        /// <summary>
        /// Commit changes asynchronously.
        /// </summary>
        /// <returns>Task.</returns>
        Task CommitAsync();

        /// <summary>
        /// Rollback last transaction.
        /// </summary>
        void Rollback();
    }
}
