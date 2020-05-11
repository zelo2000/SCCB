using System;

namespace SCCB.Core.DTO
{
    public class UserProfile
    {
        public Guid Id { get; set; }

        public string StudentId { get; set; }

        public string LectorPosition { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }
    }
}
