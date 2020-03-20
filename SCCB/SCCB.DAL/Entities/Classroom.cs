﻿using SCCB.Core.Infrastructure;
using System;
using System.Collections.Generic;

namespace SCCB.DAL.Entities
{
    public class Classroom : IIdentifiable<Guid>
    {
        public Guid Id { get; set; }

        public string Number { get; set; }

        public string Building { get; set; }

        public ICollection<Booking> Bookings { get; set; }

        public ICollection<Lesson> Lessons { get; set; }
    }
}
