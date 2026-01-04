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
            this.ErrorMessage = _validationerrormessage.stringlength_max;
        }
        public Validation_StringLengthAttribute(int maximumlength, int minimumlength) : base(maximumlength)
        {
            this.MinimumLength = minimumlength;
            this.ErrorMessage = (maximumlength == minimumlength ? _validationerrormessage.stringlength_maxminequal : _validationerrormessage.stringlength_maxmin);
        }
    }
}