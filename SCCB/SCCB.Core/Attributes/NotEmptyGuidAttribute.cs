﻿using System;
using System.ComponentModel.DataAnnotations;

namespace SCCB.Core.Attributes
{
    [AttributeUsage(
        AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter,
        AllowMultiple = false)
    ]
    public class NotEmptyGuidAttribute : ValidationAttribute
    {
        public new const string ErrorMessage = "The {0} field must not be empty";

        public NotEmptyGuidAttribute() : base(ErrorMessage) { }

        public override bool IsValid(object value)
        {
            if (value is null)
            {
                return true;
            }

            switch (value)
            {
                case Guid guid:
                    return guid != Guid.Empty;
                default:
                    return true;
            }
        }
    }
}