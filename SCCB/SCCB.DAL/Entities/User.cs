using SCCB.Core.Infrastructure;
using System;
using System.Collections.Generic;

namespace SCCB.DAL.Entities
{
    public class User : IIdentifiable<Guid>
    {
        public Guid Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public string PasswordHash { get; set; }

        public string Role { get; set; }

        public Student Student { get; set; }

        public Lector Lector { get; set; }

        public Admin Admin { get; set; }

        public ICollection<Booking> Bookings { get; set; }

        public ICollection<UsersToGroups> UsersToGroups { get; set; }
    }
}
