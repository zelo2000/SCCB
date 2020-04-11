using System;
using SCCB.DAL;
using SCCB.DAL.Entities;
using SCCB.Repos.Generic;

namespace SCCB.Repos.Classrooms
{
    public class ClassroomRepository : GenericRepository<Classroom, Guid>, IClassroomRepository
    {
        public ClassroomRepository(SCCBDbContext dbContext)
            : base(dbContext)
        {
        }
    }
}
