using System;
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
    }
}
