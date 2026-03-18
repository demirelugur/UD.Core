namespace UD.Core.Attributes.DataAnnotations
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using static UD.Core.Helper.GlobalConstants;
    using static UD.Core.Helper.OrtakTools;
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Parameter, AllowMultiple = false)]
    public sealed class UDStringLengthAttribute : StringLengthAttribute
    {
        public UDStringLengthAttribute(int maximumlength) : base(maximumlength)
        {
            this.ErrorMessage = (Guards.IsUICultureEnglish ? "{0}, must be at most {1} characters long." : ValidationErrorMessageConstants.StringLengthMax);
        }
        public UDStringLengthAttribute(int maximumlength, int minimumlength) : base(maximumlength)
        {
            this.MinimumLength = minimumlength;
            this.ErrorMessage = (maximumlength == minimumlength ? (Guards.IsUICultureEnglish ? "{0} must be exactly {1} characters long!" : ValidationErrorMessageConstants.StringLengthEqualMaxMin) : (Guards.IsUICultureEnglish ? "{0} must be at least {2} and at most {1} characters long!" : ValidationErrorMessageConstants.StringLengthBetweenMaxMin));
        }
    }
}