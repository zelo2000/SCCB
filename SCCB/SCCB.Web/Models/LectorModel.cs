using System;

namespace SCCB.Web.Models
{
    public class LectorModel
    {
        /// <summary>
        /// Currenty not in use, but may be useful in the future.
        /// </summary>
        public Guid Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Position { get; set; }
    }
}
