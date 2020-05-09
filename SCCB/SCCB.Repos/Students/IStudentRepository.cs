using System;
using System.Threading.Tasks;
using SCCB.DAL.Entities;
using SCCB.Repos.Generic;

namespace SCCB.Repos.Students
{
    public interface IStudentRepository : IGenericRepository<Student, string>
    {
        /// <summary>
        /// Find student by user identifier.
        /// </summary>
        /// <param name="userId">User identifier.</param>
        /// <returns>Student.</returns>
        Task<Student> FindStudentByUserId(Guid userId);
    }
}