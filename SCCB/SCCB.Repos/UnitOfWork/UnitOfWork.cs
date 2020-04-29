using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SCCB.DAL;
using SCCB.Repos.Bookings;
using SCCB.Repos.Classrooms;
using SCCB.Repos.Groups;
using SCCB.Repos.Lectors;
using SCCB.Repos.Lessons;
using SCCB.Repos.Users;

namespace SCCB.Repos.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly SCCBDbContext _dbContext;
        private bool disposedValue = false;

        public UnitOfWork(SCCBDbContext context)
        {
            _dbContext = context;
            Users = new UserRepository(_dbContext);
            Lessons = new LessonRepository(_dbContext);
            Groups = new GroupRepository(_dbContext);
            Classrooms = new ClassroomRepository(_dbContext);
            Lectors = new LectorRepository(_dbContext);
        }

        #region Repositories
        public IUserRepository Users { get; }

        public ILessonRepository Lessons { get; }

        public IGroupRepository Groups { get; }

        public IClassroomRepository Classrooms { get; }

        public ILectorRepository Lectors { get; }

        public IBookingRepository Bookings { get; }
        #endregion

        /// <inheritdoc />
        public async Task CommitAsync()
        {
            await _dbContext.SaveChangesAsync();
        }

        /// <inheritdoc />
        public async void Rollback()
        {
            foreach (var entry in _dbContext.ChangeTracker.Entries())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.State = EntityState.Detached;
                        break;
                    case EntityState.Modified:
                    case EntityState.Deleted:
                        await entry.ReloadAsync();
                        break;
                }
            }
        }

        #region IDisposable Support
        public void Dispose()
        {
            Dispose(true);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    _dbContext.Dispose();
                }

                disposedValue = true;
            }
        }
        #endregion
    }
}
