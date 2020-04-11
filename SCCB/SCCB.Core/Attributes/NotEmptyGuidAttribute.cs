using System;
using System.ComponentModel.DataAnnotations;

namespace SCCB.Core.Attributes
{
    /// <summary>
    /// NotEmptyGuidAttribute.
    /// </summary>
    [AttributeUsage(
        AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter,
        AllowMultiple = false)
    ]
    public class NotEmptyGuidAttribute : ValidationAttribute
    {
        private new const string ErrorMessage = "The {0} field must not be empty";

        /// <summary>
        /// Initializes a new instance of the <see cref="NotEmptyGuidAttribute"/> class.
        /// </summary>
        public NotEmptyGuidAttribute()
            : base(ErrorMessage)
        {
        }

        /// <summary>
        /// Check is Guid valid.
        /// </summary>
        /// <param name="value">Guid value.</param>
        /// <returns>True if guid valid.</returns>
        public override bool IsValid(object value)
        {
            if (value.GetType() == typeof(Guid))
            {
                return (Guid)value != Guid.Empty;
            }
            else
            {
                return false;
            }
        }
    }
}