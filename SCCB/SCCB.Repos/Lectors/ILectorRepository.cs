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
        /// Find lector by user identifier.
        /// </summary>
        /// <param name="userId">User identifier.</param>
        /// <returns>Lector.</returns>
        Task<Lector> FindLectorByUserId(Guid userId);

        /// <summary>
        /// Get all lectors with info about user.
        /// </summary>
        /// <returns>IEnumerable of lectors.</returns>
        Task<IEnumerable<Lector>> GetAllWithUserInfoAsync();

        /// <summary>
        /// Find free lectors (with user info for) given time.
        /// </summary>
        /// <param name="time">Lesson time for which to find lectors.</param>
        /// <returns>IEnumerable of free lectors.</returns>
        Task<IEnumerable<Lector>> FindFreeLectors(Core.DTO.LessonTime time);
    }
}
