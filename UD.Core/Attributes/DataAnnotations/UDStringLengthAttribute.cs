namespace UD.Core.Attributes.DataAnnotations
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using static UD.Core.Helper.GlobalConstants;
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Parameter, AllowMultiple = false)]
    public sealed class UDStringLengthAttribute : StringLengthAttribute
    {
        public UDStringLengthAttribute(int maximumlength) : base(maximumlength)
        {
            this.ErrorMessage = ValidationErrorMessageConstants.StringLengthMax;
        }
        public UDStringLengthAttribute(int maximumlength, int minimumlength) : base(maximumlength)
        {
            this.MinimumLength = minimumlength;
            this.ErrorMessage = (maximumlength == minimumlength ? ValidationErrorMessageConstants.StringLengthEqualMaxMin : ValidationErrorMessageConstants.StringLengthBetweenMaxMin);
        }
    }
}