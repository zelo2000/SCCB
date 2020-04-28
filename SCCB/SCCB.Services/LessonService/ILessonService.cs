using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SCCB.Core.DTO;

namespace SCCB.Services.LessonService
{
    public interface ILessonService
    {
        /// <summary>
        /// Get lessons by group and weekday. They will be ordered by number
        /// and grouped by it. Each lesson number corresponds to one everyweek lesson
        /// or list from two lessons (first - numerator, second - denominator).
        /// </summary>
        /// <param name="groupId">Group id.</param>
        /// <param name="weekday">Weekday.</param>
        /// <returns>Lessons list.</returns>
        Task<IReadOnlyDictionary<string, IEnumerable<Lesson>>> FindByGroupIdAndWeekday(Guid groupId, string weekday);

        /// <summary>
        /// Find list of lessons by group id.
        /// </summary>
        /// <param name="id">Group id.</param>
        /// <returns>Lessons list.</returns>
        Task<IEnumerable<Lesson>> FindByGroupId(Guid? id);

        /// <summary>
        /// Add lesson.
        /// </summary>
        /// <param name="lessonDto">Lesson.</param>
        /// <returns>Task.</returns>
        Task Add(Lesson lessonDto);

        /// <summary>
        /// Update all lesson's properties.
        /// </summary>
        /// <param name="lessonDto">Updated lesson data.</param>
        /// <returns>Task.</returns>
        Task Update(Lesson lessonDto);

        /// <summary>
        /// Remove lesson.
        /// </summary>
        /// <param name="id">Lesson's id.</param>
        /// <returns>Task.</returns>
        Task Remove(Guid id);

        /// <summary>
        /// Find lesson.
        /// </summary>
        /// <param name="id">Lesson's id.</param>
        /// <returns>Lesson.</returns>
        Task<Lesson> Find(Guid id);
    }
}
