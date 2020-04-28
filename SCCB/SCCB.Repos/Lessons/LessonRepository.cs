using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SCCB.DAL;
using SCCB.DAL.Entities;
using SCCB.Repos.Generic;

namespace SCCB.Repos.Lessons
{
    /// <summary>
    /// Repository for work with Lesson entity.
    /// </summary>
    public class LessonRepository : GenericRepository<Lesson, Guid>, ILessonRepository
    {
        private readonly SCCBDbContext _dbContext;

        /// <summary>
        /// Initializes a new instance of the <see cref="LessonRepository"/> class.
        /// </summary>
        /// <param name="dbContext">Db context instance.</param>
        public LessonRepository(SCCBDbContext dbContext)
            : base(dbContext)
        {
            _dbContext = dbContext;
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<Lesson>> FindByGroupId(Guid id)
        {
            return await _dbContext.Lessons.Where(x => x.GroupId == id).ToListAsync();
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<Lesson>> FindByGroupIdAndWeekday(Guid groupId, string weekday)
        {
            return await _dbContext.Lessons.Include(x => x.Lector)
                                                .ThenInclude(y => y.User)
                                           .Include(x => x.Classroom)
                                           .Where(x => x.GroupId == groupId && x.Weekday == weekday)
                                           .OrderBy(x => x.LessonNumber)
                                           .ToListAsync();
        }

        /// <inheritdoc/>
        public async Task<Lesson> FindByGroupIdAndTime(Guid groupId, Core.DTO.LessonTime time)
        {
            return await _dbContext.Lessons
                .Where(lesson => lesson.GroupId == groupId
                    && lesson.Weekday == time.Weekday
                    && lesson.LessonNumber == time.LessonNumber
                    && ((time.IsNumerator == time.IsDenominator)
                        || (lesson.IsEnumerator == time.IsNumerator && lesson.IsDenominator == time.IsDenominator)))
                .SingleOrDefaultAsync();
        }
    }
}
