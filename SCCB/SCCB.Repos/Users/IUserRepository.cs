using SCCB.DAL.Entities;
using SCCB.Repos.Generic;
using System;
using System.Threading.Tasks;

namespace SCCB.Repos.Users
{
    public interface IUserRepository: IGenericRepository<User, Guid> 
    {
        Task<User> FindByEmail(string email);
    }
}
