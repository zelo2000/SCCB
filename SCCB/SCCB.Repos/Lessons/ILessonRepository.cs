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
        Task<IEnumerable<Lesson>> FindByGroupId(Guid id);

        /// <summary>
        /// Get list of lessons (including lector with user info and classroom) by weekday and group, ordered by number.
        /// </summary>
        /// <param name="groupId">Group id.</param>
        /// <param name="weekday">Weekday.</param>
        /// <returns>Lessons list.</returns>
        Task<IEnumerable<Lesson>> FindByGroupIdAndWeekday(Guid groupId, string weekday);

        /// <summary>
        /// Get lesson by group id and time.
        /// </summary>
        /// <param name="groupId">Group id.</param>
        /// <param name="time">Time when lesson takes place.</param>
        /// <returns>Lesson that group with <paramref name="groupId"/> have in <paramref name="time"/>.</returns>
        Task<Lesson> FindByGroupIdAndTime(Guid groupId, Core.DTO.LessonTime time);
    }
}
