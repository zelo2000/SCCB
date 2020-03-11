﻿using SCCB.Core.Helpers;
using System;

namespace SCCB.DAL.Entities
{
    public class Admin : IIdentifiable<Guid>
    {
        public Guid Id { get; set; }

        public Guid UserId { get; set; }
        public User User { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }
    }
}