using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using SCCB.DAL.Entities;

namespace SCCB.DAL.Comparers
{
    /// <summary>
    /// IEqualtyComparer implementation for classroom comparison.
    /// </summary>
    public class ClassroomComparer : IEqualityComparer<Classroom>
    {
        /// <summary>
        /// Compare classrooms by id.
        /// </summary>
        /// <param name="x">First classroom.</param>
        /// <param name="y">Second classroom.</param>
        /// <returns>True if ids are equal, false otherwise.</returns>
        public bool Equals([AllowNull] Classroom x, [AllowNull] Classroom y)
        {
            if (x == null && y == null)
            {
                return true;
            }
            else if (x == null || y == null)
            {
                return false;
            }
            else
            {
                return x.Id == y.Id;
            }
        }

        /// <summary>
        /// Get hash code of classroom id.
        /// </summary>
        /// <param name="obj">Classroom.</param>
        /// <returns>Hash code.</returns>
        public int GetHashCode([DisallowNull] Classroom obj)
        {
            return obj.Id.GetHashCode();
        }
    }
}
