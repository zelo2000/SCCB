using SCCB.Core.Infrastructure;
using System;

namespace SCCB.DAL.Entities
{
    public class Booking : IIdentifiable<Guid>
    {
        public Guid Id { get; set; }

        public string Description { get; set; }

        public DateTime Date { get; set; }

        public int LessonNumber { get; set; }

        public Guid UserId { get; set; }
        public User User { get; set; }

        public Guid ClassroomId { get; set; }
        public Classroom Classroom { get; set; }

        public Guid? GroupId { get; set; }
        public Group Group { get; set; }
    }
}
