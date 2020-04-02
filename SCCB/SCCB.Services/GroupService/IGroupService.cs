using SCCB.Core.DTO;
using System;
using System.Threading.Tasks;

namespace SCCB.Services.GroupService
{
    public interface IGroupService
    {
        /// <summary>
        /// Add group
        /// </summary>
        /// <param name="groupDto">Group</param>
        /// <returns>Group id</returns>
        Task Add(Group groupDto);

        /// <summary>
        /// Update all group's properties
        /// </summary>
        /// <param name="groupDto">Updated group data</param>
        Task Update(Group groupDto);

        /// <summary>
        /// Remove group
        /// </summary>
        /// <param name="id">Group's id</param>
        Task Remove(Guid id);

        /// <summary>
        /// Find group
        /// </summary>
        /// <param name="id">Group's id</param>
        /// <returns>Group</returns>
        Task<Group> Find(Guid id);
    }
}
