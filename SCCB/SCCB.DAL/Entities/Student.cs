using System;
using SCCB.Core.Infrastructure;

namespace SCCB.DAL.Entities
{
    public class Student : IIdentifiable<string>
    {
        public string Id { get; set; }

        public Guid UserId { get; set; }

        public User User { get; set; }

        public Guid AcademicGroupId { get; set; }

        public Group AcademicGroup { get; set; }
    }
}
