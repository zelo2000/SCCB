using System;

namespace SCCB.Core.DTO
{
    /// <summary>
    /// Group DTO.
    /// </summary>
    public class Group
    {
        /// <summary>
        /// Gets or sets unique identifier of group.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Gets or sets group name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets unique identifier of owner.
        /// </summary>
        public Guid OwnerId { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether group is academic.
        /// </summary>
        public bool IsAcademic { get; set; }
    }
}
