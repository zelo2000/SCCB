using System.Threading.Tasks;
using SCCB.Repos.Admins;
using SCCB.Repos.Bookings;
using SCCB.Repos.Classrooms;
using SCCB.Repos.Groups;
using SCCB.Repos.Lectors;
using SCCB.Repos.Lessons;
using SCCB.Repos.Students;
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

        public IBookingRepository Bookings { get; }

        public IAdminRepository Admins { get; }

        public IStudentRepository Students { get; }
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
