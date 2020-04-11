using SCCB.Core.DTO;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SCCB.Services.LessonService
{
    public interface ILessonService
    {
        /// <summary>
        /// Get lists of lessons by weekday and group, ordered by number
        /// </summary>
        /// <param name="groupId">Group id</param>
        /// <param name="weekday">Weekday</param>
        /// <returns>Lessons list</returns>
        Task<IEnumerable<Lesson>> GetLessonsOrderedByNumber(Guid? groupId, string weekday);

        /// <summary>
        /// Find list of lessons by group id
        /// </summary>
        /// <param name="id">Group id</param>
        /// <returns>Lessons list</returns>
        Task<IEnumerable<Lesson>> FindLessonsByGroupId(Guid? id);

        /// <summary>
        /// Add lesson
        /// </summary>
        /// <param name="lessonDto">Lesson</param>
        /// <returns>Lesson id</returns>
        Task Add(Lesson lessonDto);

        /// <summary>
        /// Update all lesson's properties
        /// </summary>
        /// <param name="lessonDto">Updated lesson data</param>
        Task Update(Lesson lessonDto);

        /// <summary>
        /// Remove lesson
        /// </summary>
        /// <param name="id">Lesson's id</param>
        Task Remove(Guid id);

        /// <summary>
        /// Find lesson
        /// </summary>
        /// <param name="id">Lesson's id</param>
        /// <returns>Lesson</returns>
        Task<Lesson> Find(Guid id);
    }
}
