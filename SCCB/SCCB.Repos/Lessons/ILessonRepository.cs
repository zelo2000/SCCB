using SCCB.DAL.Entities;
using SCCB.Repos.Generic;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SCCB.Repos.Lessons
{
    public interface ILessonRepository : IGenericRepository<Lesson, Guid>
    {
        /// <summary>
        /// Find list of lessons by group id
        /// </summary>
        /// <param name="id">Group id</param>
        /// <returns>Lessons list</returns>
        Task<List<Lesson>> FindLessonsByGroupIdAsync(Guid id);
    }
}
