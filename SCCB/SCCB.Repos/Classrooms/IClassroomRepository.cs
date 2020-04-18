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
        /// Find free classrooms for given time.
        /// </summary>
        /// <param name="time">Lesson time for which to find classrooms.</param>
        /// <returns>IEnumerable of free classrooms.</returns>
        Task<IEnumerable<Classroom>> FindFreeClassrooms(Core.DTO.LessonTime time);
    }
}
