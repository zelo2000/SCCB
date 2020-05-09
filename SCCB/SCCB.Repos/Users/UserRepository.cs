using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SCCB.DAL;
using SCCB.DAL.Entities;
using SCCB.Repos.Generic;

namespace SCCB.Repos.Users
{
    public class UserRepository : GenericRepository<User, Guid>, IUserRepository
    {
        private readonly SCCBDbContext _dbContext;

        public UserRepository(SCCBDbContext dbContext)
            : base(dbContext)
        {
            _dbContext = dbContext;
        }

        /// <inheritdoc />
        public async Task<User> FindByEmailAsync(string email)
        {
            return await _dbContext.Users
                .SingleOrDefaultAsync(user => user.Email.Equals(email));
        }

        /// <inheritdoc />
        public async Task<IEnumerable<User>> FindByGroupId(Guid groupId)
        {
            return await _dbContext.UsersToGroups
                .Include(x => x.User)
                .Where(x => x.GroupId == groupId)
                .Select(x => x.User)
                .ToListAsync();
        }

        /// <inheritdoc />
        public async Task<User> FindWithLectorAndStudentInfoById(Guid id)
        {
            return await _dbContext.Users.Include(x => x.Lector)
                                         .Include(x => x.Student)
                                         .Where(x => x.Id == id)
                                         .SingleOrDefaultAsync();
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<User>> FindByRoleWithoutOwnData(string role, Guid id)
        {
            return await _dbContext.Users.Where(x => x.Role == role && x.Id != id)
                                         .OrderBy(x => x.LastName).ThenBy(y => y.FirstName)
                                         .ToListAsync();
        }
    }
}
