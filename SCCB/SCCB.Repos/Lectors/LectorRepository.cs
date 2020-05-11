using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SCCB.DAL;
using SCCB.DAL.Entities;
using SCCB.Repos.Generic;

namespace SCCB.Repos.Lectors
{
    /// <summary>
    /// Lector repository.
    /// </summary>
    public class LectorRepository : GenericRepository<Lector, Guid>, ILectorRepository
    {
        private readonly SCCBDbContext _dbContext;

        /// <summary>
        /// Initializes a new instance of the <see cref="LectorRepository"/> class.
        /// </summary>
        /// <param name="dbContext">DbContext instance.</param>
        public LectorRepository(SCCBDbContext dbContext)
            : base(dbContext)
        {
            _dbContext = dbContext;
        }

        /// <inheritdoc/>
        public async Task<Lector> FindLectorByUserId(Guid userId)
        {
            return await _dbContext.Lectors.Include(x => x.User)
                                           .Where(x => x.UserId == userId)
                                           .FirstOrDefaultAsync();
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<Lector>> GetAllWithUserInfoAsync()
        {
            return await _dbContext.Lectors.Include(lector => lector.User).ToListAsync();
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<Lector>> FindFreeLectors(Core.DTO.LessonTime time)
        {
            var busyLectors = _dbContext.Lessons
                .Where(lesson => lesson.Weekday == time.Weekday
                    && lesson.LessonNumber == time.LessonNumber
                    && ((lesson.IsEnumerator == lesson.IsDenominator)
                        || (lesson.IsEnumerator == time.IsNumerator && lesson.IsDenominator == time.IsDenominator)))
                .Select(lesson => lesson.LectorId);

            var freeLectors = await _dbContext.Lectors
                .Include(lector => lector.User)
                .Where(x => _dbContext.Lectors
                    .Select(lector => lector.Id)
                    .Except(busyLectors)
                    .Contains(x.Id))
                .ToListAsync();

            return freeLectors;
        }
    }
}
