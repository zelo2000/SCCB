using System;
using System.Collections.Generic;
using SCCB.Core.Infrastructure;

namespace SCCB.DAL.Entities
{
    public class Lector : IIdentifiable<Guid>
    {
        public Guid Id { get; set; }

        public Guid UserId { get; set; }

        public User User { get; set; }

        public string Position { get; set; }

        public ICollection<Lesson> Lessons { get; set; }
    }
}
