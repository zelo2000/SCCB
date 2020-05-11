using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SCCB.Core.DTO;

namespace SCCB.Services.ClassroomService
{
    public interface IClassroomService
    {
        /// <summary>
        /// Add classroom.
        /// </summary>
        /// <param name="classroomDto">Classroom.</param>
        /// <returns>Classroom id.</returns>
        Task Add(Classroom classroomDto);

        /// <summary>
        /// Update all classroom's properties.
        /// </summary>
        /// <param name="classroomDto">Updated classroom data.</param>
                /// <returns>Task.</returns>
        Task Update(Classroom classroomDto);

        /// <summary>
        /// Remove classroom.
        /// </summary>
        /// <param name="id">Classroom's id.</param>
        /// <returns>Task.</returns>
        Task Remove(Guid id);

        /// <summary>
        /// Find classroom.
        /// </summary>
        /// <param name="id">Classroom's id.</param>
        /// <returns>Classroom.</returns>
        Task<Classroom> Find(Guid id);

        /// <summary>
        /// Get all classrooms.
        /// </summary>
        /// <returns>IEnumerable of classrooms.</returns>
        Task<IEnumerable<Classroom>> GetAll();

        /// <summary>
        /// Get all classrooms grouped by building.
        /// </summary>
        /// <returns>ILookup of classrooms grouped by building.</returns>
        Task<IReadOnlyDictionary<string, IEnumerable<Classroom>>> GetAllGroupedByBuilding();

        /// <summary>
        /// Find free classrooms for given time.
        /// </summary>
        /// <param name="time">Lesson time for which to find classrooms.</param>
        /// <returns>IReadOnlyDictionary of free classrooms grouped by building. If weekday or lesson number is null or empty returns empty dictionary.</returns>
        Task<IReadOnlyDictionary<string, IEnumerable<Classroom>>> FindFreeClassroomsGroupedByBuilding(LessonTime time);

        /// <summary>
        /// Find free classrooms for given time.
        /// </summary>
        /// <param name="date">Date.</param>
        /// <param name="lessonNumber">Lesson number.</param>
        /// <returns>IReadOnlyDictionary of free classrooms grouped by building. If date or lesson number is null or empty returns empty dictionary.</returns>
        Task<IReadOnlyDictionary<string, IEnumerable<Classroom>>> FindFreeClassroomsGroupedByBuilding(DateTime? date, int? lessonNumber);
    }
}
