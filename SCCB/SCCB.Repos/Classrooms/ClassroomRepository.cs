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
        public async Task<IEnumerable<Classroom>> FindFreeClassrooms(Core.DTO.LessonTime time)
        {
            var classroomsInUse = _dbContext.Lessons
                .Where(lesson => lesson.Weekday == time.Weekday
                    && lesson.LessonNumber == time.LessonNumber
                    && ((lesson.IsEnumerator == lesson.IsDenominator)
                        || (lesson.IsEnumerator == time.IsNumerator && lesson.IsDenominator == time.IsDenominator)))
                .Select(lesson => lesson.ClassroomId);

            var freeClassrooms = await _dbContext.Classrooms
                .Where(x => _dbContext.Classrooms
                    .Select(classroom => classroom.Id)
                    .Except(classroomsInUse)
                    .Contains(x.Id))
                .ToListAsync();

            return freeClassrooms;
        }
    }
}
