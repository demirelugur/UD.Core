namespace UD.Core.Attributes.DataAnnotations
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using static UD.Core.Helper.GlobalConstants;
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Parameter, AllowMultiple = false)]
    public sealed class Validation_StringLengthAttribute : StringLengthAttribute
    {
        public Validation_StringLengthAttribute(int maximumlength) : base(maximumlength)
        {
            this.ErrorMessage = ValidationErrorMessageConstants.StringLengthMax;
        }
        public Validation_StringLengthAttribute(int maximumlength, int minimumlength) : base(maximumlength)
        {
            this.MinimumLength = minimumlength;
            this.ErrorMessage = (maximumlength == minimumlength ? ValidationErrorMessageConstants.StringLengthEqualMaxMin : ValidationErrorMessageConstants.StringLengthBetweenMaxMin);
        }
    }
}