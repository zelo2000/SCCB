using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SCCB.DAL.Entities;
using SCCB.Repos.Generic;

namespace SCCB.Repos.Lectors
{
    public interface ILectorRepository : IGenericRepository<Lector, Guid>
    {
        /// <summary>
        /// Get all lectors with info about user.
        /// </summary>
        /// <returns>IEnumerable of lectors.</returns>
        Task<IEnumerable<Lector>> GetAllWithUserInfoAsync();
    }
}
