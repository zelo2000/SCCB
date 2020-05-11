using System;
using System.Threading.Tasks;
using SCCB.DAL.Entities;
using SCCB.Repos.Generic;

namespace SCCB.Repos.Admins
{
    /// <summary>
    /// Interface for admin repository.
    /// </summary>
    public interface IAdminRepository : IGenericRepository<Admin, Guid>
    {
        /// <summary>
        /// Find admin by user identifier.
        /// </summary>
        /// <param name="userId">User identifier.</param>
        /// <returns>Admin.</returns>
        Task<Admin> FindAdminByUserId(Guid userId);
    }
}