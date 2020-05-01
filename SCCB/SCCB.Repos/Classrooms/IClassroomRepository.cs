using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SCCB.DAL.Entities;
using SCCB.Repos.Generic;

namespace SCCB.Repos.Classrooms
{
    public interface IClassroomRepository : IGenericRepository<Classroom, Guid>
    {
        /// <summary>
        /// Find classrooms that are assigned for any lesson at given time.
        /// </summary>
        /// <param name="time">Lesson time for which to find classrooms.</param>
        /// <returns>IEnumerable of free classrooms.</returns>
        Task<IEnumerable<Classroom>> FindClassroomsAssignedForLesson(Core.DTO.LessonTime time);

        /// <summary>
        /// Find classrooms that are booked at given time.
        /// </summary>
        /// <param name="date">Date.</param>
        /// <param name="lessonNumber">Lesson number.</param>
        /// <returns>IEnumerable of free classrooms.</returns>
        Task<IEnumerable<Classroom>> FindBookedClassrooms(DateTime date, int lessonNumber);
    }
}
