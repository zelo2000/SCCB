using SCCB.Core.Infrastructure;
using System;
using System.Collections.Generic;

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
