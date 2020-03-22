using System;
using System.Collections.Generic;
using System.Text;

namespace SCCB.Core.DTO
{
    public class UserProfile
    {
        public Guid Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }
    }
}
