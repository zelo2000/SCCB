using SCCB.DAL.Entities;
using SCCB.Repos.Generic;
using System;

namespace SCCB.Repos.Users
{
    public interface IUserRepository: IGenericRepository<User, Guid> { }
}
