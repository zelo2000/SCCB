using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SCCB.DAL;
using SCCB.DAL.Entities;
using SCCB.Repos.Generic;

namespace SCCB.Repos.Groups
{
    public class GroupRepository : GenericRepository<Group, Guid>, IGroupRepository
    {
        private readonly SCCBDbContext _dbContext;

        public GroupRepository(SCCBDbContext dbContext)
            : base(dbContext)
        {
            _dbContext = dbContext;
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<Group>> FindByIsAcademic(bool isAcademic)
        {
            return await _dbContext.Groups.Where(x => x.IsAcademic == isAcademic).ToListAsync();
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<Group>> FindNotAcademic(Guid userId, bool isUserOwner)
        {
            return await _dbContext.UsersToGroups
                .Include(x => x.Group)
                .Where(x => x.UserId == userId
                    && x.IsUserOwner == isUserOwner
                    && x.Group.IsAcademic == false)
                .Select(x => x.Group)
                .ToListAsync();
        }

        /// <inheritdoc/>
        public Guid AddUser(UsersToGroups userToGroup)
        {
            var entity = _dbContext.UsersToGroups.Add(userToGroup);
            return entity.Entity.Id;
        }

        /// <inheritdoc/>
        public void RemoveUser(UsersToGroups userToGroup)
        {
            _dbContext.UsersToGroups.Remove(userToGroup);
        }

        /// <inheritdoc/>
        public async Task<Guid> GetOwner(Guid groupId)
        {
            return await _dbContext.UsersToGroups
                .Where(x => x.GroupId == groupId && x.IsUserOwner == true)
                .Select(x => x.UserId)
                .SingleOrDefaultAsync();
        }

        public async Task<UsersToGroups> FindUserToGroup(Guid userId, Guid groupId)
        {
            return await _dbContext.UsersToGroups
                .Where(x => x.UserId == userId && x.GroupId == groupId)
                .SingleOrDefaultAsync();
        }
    }
}
