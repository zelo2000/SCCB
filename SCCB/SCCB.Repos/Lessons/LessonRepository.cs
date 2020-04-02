using Microsoft.EntityFrameworkCore;
using SCCB.DAL;
using SCCB.DAL.Entities;
using SCCB.Repos.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SCCB.Repos.Lessons
{
    public class LessonRepository : GenericRepository<Lesson, Guid>, ILessonRepository
    {
        private readonly SCCBDbContext _dbContext;

        public LessonRepository(SCCBDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<Lesson>> FindLessonsByGroupIdAsync(Guid id)
        {
            return await _dbContext.Lessons.Where(x => x.GroupId == id).ToListAsync();
        }
    }
}
