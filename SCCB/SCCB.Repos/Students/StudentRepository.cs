using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SCCB.DAL;
using SCCB.DAL.Entities;
using SCCB.Repos.Generic;

namespace SCCB.Repos.Students
{
    /// <summary>
    /// Student repository.
    /// </summary>
    public class StudentRepository : GenericRepository<Student, string>, IStudentRepository
    {
        private readonly SCCBDbContext _dbContext;

        /// <summary>
        /// Initializes a new instance of the <see cref="StudentRepository"/> class.
        /// </summary>
        /// <param name="dbContext">DbContext instance.</param>
        public StudentRepository(SCCBDbContext dbContext)
            : base(dbContext)
        {
            _dbContext = dbContext;
        }

        /// <inheritdoc/>
        public async Task<Student> FindStudentByUserId(Guid userId)
        {
            return await _dbContext.Students.Include(x => x.User)
                                            .FirstOrDefaultAsync(x => x.UserId == userId);
        }
    }
}
