﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SCCB.DAL.Entities;
using SCCB.Repos.Generic;

namespace SCCB.Repos.Bookings
{
    public interface IBookingRepository : IGenericRepository<Booking, Guid>
    {
        /// <summary>
        /// Find booking by date, lesson number, and classroom GUID. Search parameter is omitted if it's value is null.
        /// </summary>
        /// <param name="date">Date.</param>
        /// <param name="lessonNumber">Lesson number.</param>
        /// <param name="classroomId">Classroom id.</param>
        /// <returns>IEnumerable of bookings with user, classroom and group included.</returns>
        Task<IEnumerable<Booking>> FindBookingsWithIncludedInfo(DateTime? date, int? lessonNumber, Guid? classroomId);
    }
}