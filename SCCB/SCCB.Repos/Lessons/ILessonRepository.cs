using SCCB.DAL.Entities;
using SCCB.DAL;
using SCCB.Repos.Generic;
using System;
using System.Collections.Generic;
using System.Text;

namespace SCCB.Repos.Lessons
{
    public interface ILessonRepository : IGenericRepository<Lesson, Guid>
    {

    }
}
