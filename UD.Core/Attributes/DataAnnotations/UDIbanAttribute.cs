namespace UD.Core.Attributes.DataAnnotations
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using UD.Core.Extensions;
    using UD.Core.Helper;
    using static UD.Core.Helper.GlobalConstants;
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Parameter, AllowMultiple = false)]
    public sealed class UDIbanAttribute : ValidationAttribute
    {
        public UDIbanAttribute() { }
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var valueString = value.ToStringOrEmpty().ToUpper();
            if (valueString == "" && !validationContext.IsRequiredAttribute())
            {
                validationContext.SetValidatePropertyValue(null);
                return ValidationResult.Success;
            }
            if (Checks.IsIbanValid(valueString))
            {
                validationContext.SetValidatePropertyValue(valueString);
                return ValidationResult.Success;
            }
            if (this.ErrorMessage.IsNullOrEmpty())
            {
                this.ErrorMessage = $"{validationContext.DisplayName}, {TitleConstants.Iban} biçimine uygun olmalıdır!";
                if (Checks.IsEnglishCurrentUICulture) { this.ErrorMessage = $"{validationContext.DisplayName} must be in a valid IBAN format!"; }
            }
            return new(this.ErrorMessage, [validationContext.MemberName]);
        }
    }
}