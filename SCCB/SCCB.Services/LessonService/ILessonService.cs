using SCCB.Core.DTO;
using System;
using System.Threading.Tasks;

namespace SCCB.Services.LessonService
{
    public interface ILessonService
    {
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
