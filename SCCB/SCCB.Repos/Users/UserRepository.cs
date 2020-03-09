using SCCB.DAL;
using SCCB.DAL.Entities;
using System;

namespace SCCB.Repos.Users
{
    public class UserRepository : GenericRepository<User, Guid>, IUserRepository
    {
        private readonly SCCBDbContext _dbContext;

        public UserRepository(SCCBDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }
    }
}
