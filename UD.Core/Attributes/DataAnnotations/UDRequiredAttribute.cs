namespace UD.Core.Attributes.DataAnnotations
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using static UD.Core.Helper.GlobalConstants;
    using UD.Core.Helper;
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Parameter, AllowMultiple = false)]
    public sealed class UDRequiredAttribute : RequiredAttribute
    {
        public UDRequiredAttribute()
        {
            this.AllowEmptyStrings = false;
            this.ErrorMessage = (Guards.IsEnglishDefaultThreadCurrentUICulture ? "{0} cannot be left blank!" : ValidationErrorMessageConstants.Required);
        }
    }
}