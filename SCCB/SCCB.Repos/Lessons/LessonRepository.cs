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
    public class LessonRepository : GenericRepository<Lesson, Guid>, ILessonRepository
    {
        private readonly SCCBDbContext _dbContext;

        public LessonRepository(SCCBDbContext dbContext)
            : base(dbContext)
        {
            _dbContext = dbContext;
        }

        /// <inheritdoc/>
        public async Task<List<Lesson>> FindByGroupId(Guid id)
        {
            return await _dbContext.Lessons.Where(x => x.GroupId == id).ToListAsync();
        }
        
        /// <inheritdoc/>
        public async Task<List<Lesson>> FindByGroupIdAndWeekday(Guid groupId, string weekday)
        {
            return await _dbContext.Lessons.Include(x => x.Lector)
                                                .ThenInclude(y => y.User)
                                           .Include(x => x.Classroom)
                                           .Where(x => x.GroupId == groupId && x.Weekday == weekday)
                                           .OrderBy(x => x.LessonNumber)
                                           .ToListAsync();
        }
    }
}
