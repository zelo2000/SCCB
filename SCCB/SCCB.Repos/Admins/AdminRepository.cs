using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SCCB.DAL;
using SCCB.DAL.Entities;
using SCCB.Repos.Generic;

namespace SCCB.Repos.Admins
{
    /// <summary>
    /// Admin repository.
    /// </summary>
    public class AdminRepository : GenericRepository<Admin, Guid>, IAdminRepository
    {
        private readonly SCCBDbContext _dbContext;

        /// <summary>
        /// Initializes a new instance of the <see cref="AdminRepository"/> class.
        /// </summary>
        /// <param name="dbContext">DbContext instance.</param>
        public AdminRepository(SCCBDbContext dbContext)
            : base(dbContext)
        {
            _dbContext = dbContext;
        }

        /// <inheritdoc/>
        public async Task<Admin> FindAdminByUserId(Guid userId)
        {
            return await _dbContext.Admins.Include(x => x.User)
                                          .Where(x => x.UserId == userId)
                                          .FirstOrDefaultAsync();
        }
    }
}
