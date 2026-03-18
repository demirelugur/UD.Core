namespace UD.Core.Attributes.DataAnnotations
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using static UD.Core.Helper.GlobalConstants;
    using static UD.Core.Helper.OrtakTools;
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Parameter, AllowMultiple = false)]
    public sealed class UDRequiredAttribute : RequiredAttribute
    {
        public UDRequiredAttribute()
        {
            this.AllowEmptyStrings = false;
            this.ErrorMessage = (Guards.IsUICultureEnglish ? "{0} cannot be left blank!" : ValidationErrorMessageConstants.Required);
        }
    }
}