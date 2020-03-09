using SCCB.Core.Helpers;
using System;
using System.Collections.Generic;

namespace SCCB.DAL.Entities
{
    public class User : IIdentifiable<Guid>
    {
        public Guid Id { get; set; }

        public string Email { get; set; }

        public string PasswordHash { get; set; }

        public string Role { get; set; }

        public ICollection<Student> Students { get; set; }

        public ICollection<Lector> Lectors { get; set; }

        public ICollection<Admin> Admins { get; set; }

        public ICollection<Booking> Bookings { get; set; }

        public ICollection<UsersToGroups> UsersToGroups { get; set; }
    }
}
