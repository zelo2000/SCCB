using Microsoft.EntityFrameworkCore;
using SCCB.DAL;
using SCCB.DAL.Entities;
using SCCB.Repos.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
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

        public async Task<IEnumerable<Group>> FindByIsAcademic(bool isAcademic)
        {
            return await _dbContext.Groups.Where(x => x.IsAcademic == isAcademic).ToListAsync();
        }
    }
}
