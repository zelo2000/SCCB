using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SCCB.DAL.Entities;
using SCCB.Repos.Generic;

namespace SCCB.Repos.Lessons
{
    public interface ILessonRepository : IGenericRepository<Lesson, Guid>
    {
        /// <summary>
        /// Find list of lessons by group id.
        /// </summary>
        /// <param name="id">Group id.</param>
        /// <returns>Lessons list.</returns>
        Task<List<Lesson>> FindByGroupId(Guid id);

        /// <summary>
        /// Get lists of lessons by weekday and group, ordered by number.
        /// </summary>
        /// <param name="groupId">Group id.</param>
        /// <param name="weekday">Weekday.</param>
        /// <returns>Lessons list.</returns>
        Task<List<Lesson>> FindByGroupIdAndWeekday(Guid groupId, string weekday);
    }
}
