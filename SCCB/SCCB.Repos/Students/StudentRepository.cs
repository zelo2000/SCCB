using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SCCB.DAL;
using SCCB.DAL.Entities;
using SCCB.Repos.Generic;

namespace SCCB.Repos.Students
{
    public class StudentRepository : GenericRepository<Student, string>, IStudentRepository
    {
        private readonly SCCBDbContext _dbContext;

        public StudentRepository(SCCBDbContext dbContext)
            : base(dbContext)
        {
            _dbContext = dbContext;
        }

        /// <inheritdoc/>
        public async Task<Student> FindStudentByUserId(Guid userId)
        {
            return await _dbContext.Students.Include(x => x.User)
                                            .Where(x => x.UserId == userId)
                                            .FirstOrDefaultAsync();
        }
    }
}
