using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SCCB.DAL;
using SCCB.DAL.Entities;
using SCCB.Repos.Generic;

namespace SCCB.Repos.Classrooms
{
    public class ClassroomRepository : GenericRepository<Classroom, Guid>, IClassroomRepository
    {
        private readonly SCCBDbContext _dbContext;

        public ClassroomRepository(SCCBDbContext dbContext)
            : base(dbContext)
        {
            _dbContext = dbContext;
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<Classroom>> FindClassroomsAssignedForLesson(Core.DTO.LessonTime time)
        {
            return await _dbContext.Lessons
                .Where(lesson => lesson.Weekday == time.Weekday
                    && lesson.LessonNumber == time.LessonNumber
                    && ((lesson.IsEnumerator == lesson.IsDenominator)
                        || (lesson.IsEnumerator == time.IsNumerator && lesson.IsDenominator == time.IsDenominator)))
                .Select(lesson => lesson.Classroom)
                .ToListAsync();
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<Classroom>> FindBookedClassrooms(DateTime date, int lessonNumber)
        {
            return await _dbContext.Bookings
                .Where(booking => booking.Date == date
                    && booking.LessonNumber == booking.LessonNumber)
                .Select(lesson => lesson.Classroom)
                .ToListAsync();
        }
    }
}
