using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SCCB.Core.DTO;

namespace SCCB.Services.GroupService
{
    public interface IGroupService
    {
        /// <summary>
        /// Add group.
        /// </summary>
        /// <param name="groupDto">Group.</param>
        /// <returns>Group id.</returns>
        Task Add(Group groupDto);

        /// <summary>
        /// Update all group's properties.
        /// </summary>
        /// <param name="groupDto">Updated group data.</param>
        /// <returns>Task.</returns>
        Task Update(Group groupDto);

        /// <summary>
        /// Remove group.
        /// </summary>
        /// <param name="id">Group's id.</param>
        /// <returns>Task.</returns>
        Task Remove(Guid id);

        /// <summary>
        /// Find group.
        /// </summary>
        /// <param name="id">Group's id.</param>
        /// <returns>Group.</returns>
        Task<Group> Find(Guid id);

        /// <summary>
        /// Get all group's.
        /// </summary>
        /// <returns>Group.</returns>
        Task<IEnumerable<Group>> GetAll();

        /// <summary>
        /// Find group by is academic.
        /// </summary>
        /// <returns>Group.</returns>
        Task<IEnumerable<Group>> GetAllAcademic();

        /// <summary>
        /// Find groups by option.
        /// </summary>
        /// <param name="option">Which groups to select. Possible options <see cref="SCCB.Core.Constants.GroupOptions"/>.</param>
        /// <returns>IEnumerable of groups.</returns>
        Task<IEnumerable<Group>> FindByOption(string option);
    }
}
