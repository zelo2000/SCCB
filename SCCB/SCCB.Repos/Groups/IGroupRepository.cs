using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SCCB.DAL.Entities;
using SCCB.Repos.Generic;

namespace SCCB.Repos.Groups
{
    public interface IGroupRepository : IGenericRepository<Group, Guid>
    {
        /// <summary>
        /// Find group's by is academic.
        /// </summary>
        /// <param name="isAcademic">Group.</param>
        /// <returns>Group list.</returns>
        Task<IEnumerable<Group>> FindByIsAcademic(bool isAcademic);
    }
}
