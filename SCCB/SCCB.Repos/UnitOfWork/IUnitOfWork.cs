﻿using SCCB.Repos.Classrooms;
using SCCB.Repos.Groups;
using SCCB.Repos.Lectors;
using SCCB.Repos.Lessons;
using SCCB.Repos.Users;
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
        public ILectorRepository Lectors { get; }
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
