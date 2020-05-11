using System;
using System.Collections.Generic;
using System.Text;

namespace SCCB.Core.Constants
{
    /// <summary>
    /// Options for group select.
    /// </summary>
    public static class GroupOptions
    {
        /// <summary>
        /// All groups.
        /// </summary>
        public const string All = "All";

        /// <summary>
        /// Academic groups.
        /// </summary>
        public const string Academic = "Academic";

        /// <summary>
        /// User defined groups (All that are not academic).
        /// </summary>
        public const string UserDefined = "UserDefined";
    }
}
