using SCCB.DAL.Entities;
using SCCB.Repos.Generic;
using System;
using System.Collections.Generic;
using System.Text;

namespace SCCB.Repos.Groups
{
    public interface IGroupRepository : IGenericRepository<Group, Guid>
    {
    }
}
