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
        /// Find groups by is academic.
        /// </summary>
        /// <param name="isAcademic">Group.</param>
        /// <returns>Group list.</returns>
        Task<IEnumerable<Group>> FindByIsAcademic(bool isAcademic);

        /// <summary>
        /// Find groups 
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<IEnumerable<Group>> FindNotAcademicByUserId(Guid userId);
    }
}
