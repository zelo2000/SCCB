using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SCCB.DAL.Entities;
using SCCB.Repos.Generic;

namespace SCCB.Repos.Users
{
    public interface IUserRepository : IGenericRepository<User, Guid>
    {
        /// <summary>
        /// Find user by email.
        /// </summary>
        /// <param name="email">Email address.</param>
        /// <returns>User.</returns>
        Task<User> FindByEmailAsync(string email);

        /// <summary>
        /// Find user with lector info by id.
        /// </summary>
        /// <param name="id">User identifier.</param>
        /// <returns>User.</returns>
        Task<User> FindWithLectorInfoById(Guid id);

        /// <summary>
        /// Find list of users by role.
        /// </summary>
        /// <param name="role">User role.</param>
        /// <returns>Users list.</returns>
        Task<IEnumerable<User>> FindByRoleWithoutOwnData(string role, Guid id);
    }
}
