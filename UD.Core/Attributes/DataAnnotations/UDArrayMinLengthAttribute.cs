namespace UD.Core.Attributes.DataAnnotations
{
    using System.ComponentModel.DataAnnotations;
    using UD.Core.Helper;
    using static UD.Core.Helper.GlobalConstants;
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Parameter, AllowMultiple = false)]
    public sealed class UDArrayMinLengthAttribute : MinLengthAttribute
    {
        public UDArrayMinLengthAttribute() : this(1) { }
        public UDArrayMinLengthAttribute(int minimumLength) : base(minimumLength)
        {
            this.ErrorMessage = minimumLength > 1 ? (ValidationChecks.IsEnglishCurrentUICulture ? "{0} cannot be left blank! It must contain at least {1} element." : ValidationErrorMessageConstants.ArrayMinLength) : (ValidationChecks.IsEnglishCurrentUICulture ? "{0} cannot be left blank!" : ValidationErrorMessageConstants.Required);
        }
    }
}