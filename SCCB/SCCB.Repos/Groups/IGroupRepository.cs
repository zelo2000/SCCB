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
        /// Find user's not academic groups.
        /// </summary>
        /// <param name="userId">Id of user for whom to find groups.</param>
        /// <param name="isUserOwner">A value indicating whether to search groups where user is owner (true), or ordinary member (false).</param>
        /// <returns>IEnumerable of groups.</returns>
        Task<IEnumerable<Group>> FindNotAcademic(Guid userId, bool isUserOwner);

        /// <summary>
        /// Add user to group.
        /// </summary>
        /// <param name="userToGroup"><see cref="UsersToGroups"/> entity connecting user id and group id.</param>
        /// <returns>Id of added entity.</returns>
        Guid AddUser(UsersToGroups userToGroup);

        /// <summary>
        /// Remove user from group.
        /// </summary>
        /// <param name="userToGroup"><see cref="UsersToGroups"/> entity connecting user id and group id.</param>
        void RemoveUser(UsersToGroups userToGroup);

        /// <summary>
        /// Get owner of the group.
        /// </summary>
        /// <param name="groupId">Group Id.</param>
        /// <returns>Owner Id.</returns>
        Task<Guid> GetOwner(Guid groupId);

        /// <summary>
        /// Find entity connection user with <paramref name="userId"/> and group with <paramref name="groupId"/>.
        /// </summary>
        /// <param name="userId">User Id.</param>
        /// <param name="groupId">Group Id.</param>
        /// <returns><see cref="UsersToGroups"/> entity.</returns>
        Task<UsersToGroups> FindUserToGroup(Guid userId, Guid groupId);
    }
}
