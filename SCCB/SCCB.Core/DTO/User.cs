using System;

namespace SCCB.Core.DTO
{
    /// <summary>
    /// User dto.
    /// </summary>
    public class User
    {
        /// <summary>
        /// Gets or sets Id.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Gets or sets First Name.
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// Gets or sets Last Name.
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// Gets or sets Email.
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Gets or sets Password.
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// Gets or sets Role.
        /// </summary>
        public string Role { get; set; }

        /// <summary>
        /// Gets or sets Lector Position.
        /// </summary>
        public string Position { get; set; }

        /// <summary>
        /// Gets or sets Student identifier.
        /// </summary>
        public string StudentId { get; set; }

        /// <summary>
        /// Gets or sets Student GroupId.
        /// </summary>
        public Guid? AcademicGroupId { get; set; }
    }
}
