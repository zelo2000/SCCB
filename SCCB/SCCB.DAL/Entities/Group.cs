using SCCB.Core.Helpers;
using System;
using System.Collections.Generic;

namespace SCCB.DAL.Entities
{
    public class Group : IIdentifiable<Guid>
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public bool IsAcademic { get; set; }

        public ICollection<UsersToGroups> UsersToGroups { get; set; }

        public ICollection<Booking> Bookings { get; set; }

        public ICollection<Lesson> Lessons { get; set; }
    }
}
