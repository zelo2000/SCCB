using Microsoft.EntityFrameworkCore;
using SCCB.DAL;
using SCCB.DAL.Entities;
using SCCB.Repos.Generic;
using System;
using System.Threading.Tasks;

namespace SCCB.Repos.Groups
{
    public class GroupRepository : GenericRepository<Group, Guid>, IGroupRepository
    {
        private readonly SCCBDbContext _dbContext;

        public GroupRepository(SCCBDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }
    }
}
