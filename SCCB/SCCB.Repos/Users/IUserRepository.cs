using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SCCB.DAL.Entities;
using SCCB.Repos.Generic;

namespace SCCB.Repos.Users
{
    /// <summary>
    /// Interface for user repository.
    /// </summary>
    public interface IUserRepository : IGenericRepository<User, Guid>
    {
        /// <summary>
        /// Find user by email.
        /// </summary>
        /// <param name="email">Email address.</param>
        /// <returns>User.</returns>
        Task<User> FindByEmailAsync(string email);

        /// <summary>
        /// Find user with lector and student info by id.
        /// </summary>
        /// <param name="id">User identifier.</param>
        /// <returns>User.</returns>
        Task<User> FindWithLectorAndStudentInfoById(Guid id);

        /// <summary>
        /// Find list of users by role.
        /// </summary>
        /// <param name="role">User role.</param>
        /// <returns>Users list.</returns>
        Task<IEnumerable<User>> FindByRole(string role);

        /// <summary>
        /// Find users that are memebers of group with <paramref name="groupId"/>.
        /// </summary>
        /// <param name="groupId">Group id.</param>
        /// <returns>IEnumerable of <see cref="User"/>.</returns>
        Task<IEnumerable<User>> FindByGroupId(Guid groupId);

        /// <summary>
        /// Find user by role without own data.
        /// </summary>
        /// <param name="role">String role.</param>
        /// <param name="id">User id.</param>
        /// <returns>User list.</returns>
        Task<IEnumerable<User>> FindByRoleWithoutOwnData(string role, Guid id);
    }
}
