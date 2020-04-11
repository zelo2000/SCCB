using System;
using SCCB.Core.Infrastructure;

namespace SCCB.DAL.Entities
{
    public class UsersToGroups : IIdentifiable<Guid>
    {
        public Guid Id { get; set; }

        public Guid UserId { get; set; }

        public User User { get; set; }

        public Guid GroupId { get; set; }

        public Group Group { get; set; }
    }
}
