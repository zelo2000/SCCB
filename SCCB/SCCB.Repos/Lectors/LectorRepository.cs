using Microsoft.EntityFrameworkCore;
using SCCB.DAL;
using SCCB.DAL.Entities;
using SCCB.Repos.Generic;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SCCB.Repos.Lectors
{
    public class LectorRepository : GenericRepository<Lector, Guid>, ILectorRepository
    {
        private readonly SCCBDbContext _dbContext;

        public LectorRepository(SCCBDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<Lector>> GetAllWithUserInfoAsync()
        {
            return await _dbContext.Lectors.Include(lector => lector.User).ToListAsync();
        }
    }
}
