using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SCCB.Services.LectorService
{
    public interface ILectorService
    {
        /// <summary>
        /// Get all lectors
        /// </summary>
        /// <returns>IEnumerable of lectors</returns>
        Task<IEnumerable<Core.DTO.Lector>> GetAllWithUserInfo();
    }
}
