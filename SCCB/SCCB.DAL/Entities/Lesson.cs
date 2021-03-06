﻿using System;
using SCCB.Core.Infrastructure;

namespace SCCB.DAL.Entities
{
    public class Lesson : IIdentifiable<Guid>
    {
        public Guid Id { get; set; }

        public string Title { get; set; }

        public Guid GroupId { get; set; }

        public Group Group { get; set; }

        public Guid LectorId { get; set; }

        public Lector Lector { get; set; }

        public bool IsDenominator { get; set; }

        public bool IsEnumerator { get; set; }

        public string Type { get; set; }

        public string Weekday { get; set; }

        public int LessonNumber { get; set; }

        public Guid ClassroomId { get; set; }

        public Classroom Classroom { get; set; }
    }
}
