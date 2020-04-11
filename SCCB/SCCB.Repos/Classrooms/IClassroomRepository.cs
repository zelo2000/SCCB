using SCCB.DAL.Entities;
using SCCB.Repos.Generic;
using System;

namespace SCCB.Repos.Classrooms
{
    public interface IClassroomRepository : IGenericRepository<Classroom, Guid>
    {
    }
}
