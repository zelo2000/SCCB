using System.Collections.Generic;
using System.Threading.Tasks;
using SCCB.Core.DTO;

namespace SCCB.Services.LectorService
{
    /// <summary>
    /// Interface for LectorService.
    /// </summary>
    public interface ILectorService
    {
        /// <summary>
        /// Get all lectors.
        /// </summary>
        /// <returns>IEnumerable of lectors.</returns>
        Task<IEnumerable<Lector>> GetAllWithUserInfo();

        /// <summary>
        /// Find free lectors for given time.
        /// </summary>
        /// <param name="time">Lesson time for which to find lectors.</param>
        /// <returns>IEnumerable of free lectors. If weekday or lesson number is null or empty returns empty list.</returns>
        Task<IEnumerable<Lector>> FindFreeLectors(LessonTime time);
    }
}
