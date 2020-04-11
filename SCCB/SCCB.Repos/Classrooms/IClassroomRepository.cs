using System;
using SCCB.DAL.Entities;
using SCCB.Repos.Generic;

namespace SCCB.Repos.Classrooms
{
    public interface IClassroomRepository : IGenericRepository<Classroom, Guid>
    {
    }
}
