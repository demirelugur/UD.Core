namespace UD.Core.Attributes.DataAnnotations
{
    using System.ComponentModel.DataAnnotations;
    using UD.Core.Extensions;
    using UD.Core.Helper;
    using static UD.Core.Helper.GlobalConstants;
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Parameter, AllowMultiple = false)]
    public sealed class UDMacAddressAttribute : ValidationAttribute
    {
        public UDMacAddressAttribute() { }
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var valueString = value.ToStringOrEmpty();
            if (valueString == "" && !validationContext.IsRequiredAttribute())
            {
                validationContext.SetValidatePropertyValue(null);
                return ValidationResult.Success;
            }
            if (TryValidators.TryMACAddress(valueString, out string _mac))
            {
                validationContext.SetValidatePropertyValue(_mac);
                return ValidationResult.Success;
            }
            if (this.ErrorMessage.IsNullOrEmpty())
            {
                this.ErrorMessage = $"{validationContext.DisplayName}, {TitleConstants.Mac} Adresi biçimine uygun olmalıdır!";
                if (ValidationChecks.IsEnglishCurrentUICulture) { this.ErrorMessage = $"{validationContext.DisplayName} must be in a valid {TitleConstants.Mac} Address format!"; }
            }
            return new(this.ErrorMessage, [validationContext.MemberName]);
        }
    }
}