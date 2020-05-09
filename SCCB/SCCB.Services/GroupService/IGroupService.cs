using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SCCB.Core.DTO;

namespace SCCB.Services.GroupService
{
    /// <summary>
    /// IGroupService.
    /// </summary>
    public interface IGroupService
    {
        /// <summary>
        /// Add group.
        /// </summary>
        /// <param name="groupDto">Group.</param>
        /// <returns>Group id.</returns>
        Task<Guid> Add(Group groupDto);

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

        /// <summary>
        /// Find user's not academic groups.
        /// </summary>
        /// <param name="userId">Id of user for whom to find groups.</param>
        /// <param name="isUserOwner">A value indicating whether to search groups where user is owner (true), or ordinary member (false).</param>
        /// <returns>IEnumerable of groups.</returns>
        Task<IEnumerable<Group>> FindNotAcademic(Guid userId, bool isUserOwner);

        /// <summary>
        /// Find users that are members of group with <paramref name="groupId"/>.
        /// </summary>
        /// <param name="groupId">Group id.</param>
        /// <returns>IEnumerable of <see cref="UserProfile"/>.</returns>
        Task<IEnumerable<UserProfile>> FindUsersInGroup(Guid groupId);

        /// <summary>
        /// Find users that are not members group with <paramref name="groupId"/>.
        /// </summary>
        /// <param name="groupId">Group id.</param>
        /// <returns>IEnumerable of <see cref="UserProfile"/>.</returns>
        Task<IEnumerable<UserProfile>> FindUsersNotInGroup(Guid groupId);

        /// <summary>
        /// Check if user with <paramref name="userId"/> is owner of the group with <paramref name="groupId"/>.
        /// </summary>
        /// <param name="userId">User Id.</param>
        /// <param name="groupId">Group Id.</param>
        /// <returns>True if user is owner of the group, false otherwise.</returns>
        Task<bool> CheckOwnership(Guid userId, Guid groupId);

        Task<Guid> AddUser(Guid userId, Guid groupId);

        Task RemoveUser(Guid userId, Guid groupId);
    }
}
