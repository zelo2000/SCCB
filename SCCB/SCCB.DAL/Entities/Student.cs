using SCCB.Core.Infrastructure;
using System;

namespace SCCB.DAL.Entities
{
    public class Student : IIdentifiable<string>
    {
        public string Id { get; set; }

        public Guid UserId { get; set; }
        public User User { get; set; }
    }
}
