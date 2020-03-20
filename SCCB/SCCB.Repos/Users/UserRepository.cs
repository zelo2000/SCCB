using Microsoft.EntityFrameworkCore;
using SCCB.DAL;
using SCCB.DAL.Entities;
using SCCB.Repos.Generic;
using System;
using System.Threading.Tasks;

namespace SCCB.Repos.Users
{
    public class UserRepository : GenericRepository<User, Guid>, IUserRepository
    {
        private readonly SCCBDbContext _dbContext;

        public UserRepository(SCCBDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<User> FindByEmailAsync(string email)
        {
            return await _dbContext.Users
                .FirstOrDefaultAsync(user => user.Email.Equals(email));
        }
    }
}
