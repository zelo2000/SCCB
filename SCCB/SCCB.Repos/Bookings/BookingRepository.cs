using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SCCB.DAL;
using SCCB.DAL.Entities;
using SCCB.Repos.Generic;

namespace SCCB.Repos.Bookings
{
    /// <summary>
    /// Repository for work with Booking entity.
    /// </summary>
    public class BookingRepository : GenericRepository<Booking, Guid>, IBookingRepository
    {
        private readonly SCCBDbContext _dbContext;

        /// <summary>
        /// Initializes a new instance of the <see cref="BookingRepository"/> class.
        /// </summary>
        /// <param name="dbContext">Db context instance.</param>
        public BookingRepository(SCCBDbContext dbContext)
            : base(dbContext)
        {
            _dbContext = dbContext;
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<Booking>> FindBookingsWithIncludedInfo(DateTime? date, int? lessonNumber, Guid? classroomId)
        {
            var bookings = _dbContext.Bookings
                .Include(booking => booking.User)
                .Include(booking => booking.Classroom)
                .Include(booking => booking.Group)
                .Select(booking => booking);

            if (date != null)
            {
                bookings = bookings.Where(booking => booking.Date == date);
            }

            if (lessonNumber != null)
            {
                bookings = bookings.Where(booking => booking.LessonNumber == lessonNumber);
            }

            if (classroomId != null)
            {
                bookings = bookings.Where(x => x.ClassroomId == classroomId);
            }

            return await bookings.ToListAsync();
        }
    }
}
