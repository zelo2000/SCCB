using System;

namespace SCCB.Core.DTO
{
    public class Lesson
    {
        public Guid Id { get; set; }
        public Lector Lector { get; set; }
        public Classroom Classroom { get; set; }

        public string Title { get; set; }

        public Guid GroupId { get; set; }

        public Guid LectorId { get; set; }

        public bool IsDenominator { get; set; }

        public bool IsEnumerator { get; set; }

        public string Type { get; set; }

        public string Weekday { get; set; }

        public string LessonNumber { get; set; }

        public Guid ClassroomId { get; set; }
    }
}
