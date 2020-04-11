using System;

namespace SCCB.Core.DTO
{
    /// <summary>
    /// Classroom DTO.
    /// </summary>
    public class Classroom
    {
        /// <summary>
        /// Gets or sets unique identifier of classroom.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Gets or sets classroom number.
        /// </summary>
        public string Number { get; set; }

        /// <summary>
        /// Gets or sets classroom building.
        /// </summary>
        public string Building { get; set; }
    }
}
