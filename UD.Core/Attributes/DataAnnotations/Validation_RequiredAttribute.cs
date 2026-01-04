namespace UD.Core.Attributes.DataAnnotations
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using static UD.Core.Helper.GlobalConstants;
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Parameter, AllowMultiple = false)]
    public sealed class Validation_RequiredAttribute : RequiredAttribute
    {
        public Validation_RequiredAttribute()
        {
            this.AllowEmptyStrings = false;
            this.ErrorMessage = _validationerrormessage.required;
        }
    }
}