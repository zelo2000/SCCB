using SCCB.DAL.Entities;
using SCCB.Repos.Generic;
using System;
using System.Collections.Generic;
using System.Text;

namespace SCCB.Repos.Classrooms
{
    public interface IClassroomRepository: IGenericRepository<Classroom, Guid>
    {
    }
}
