using SCCB.DAL.Entities;
using SCCB.Repos.Generic;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SCCB.Repos.Groups
{
    public interface IGroupRepository : IGenericRepository<Group, Guid>
    {
        /// <summary>
        /// Find group's by is academic
        /// </summary>
        /// <param name="isAcademic">Group</param>
        Task<IEnumerable<Group>> FindByIsAcademic(bool isAcademic);
    }
}
