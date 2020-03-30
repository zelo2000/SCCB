using Microsoft.EntityFrameworkCore;
using SCCB.DAL;
using SCCB.DAL.Entities;
using SCCB.Repos.Generic;
using System;
using System.Threading.Tasks;

namespace SCCB.Repos.Classrooms
{
    public class ClassroomRepository : GenericRepository<Classroom, Guid>, IClassroomRepository
    {
        private readonly SCCBDbContext _dbContext;

        public ClassroomRepository(SCCBDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }
    }
}
