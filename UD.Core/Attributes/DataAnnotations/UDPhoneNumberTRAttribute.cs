namespace UD.Core.Attributes.DataAnnotations
{
    using System.ComponentModel.DataAnnotations;
    using UD.Core.Extensions;
    using UD.Core.Extensions.Common;
    using UD.Core.Helper;
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Parameter, AllowMultiple = false)]
    public sealed class UDPhoneNumberTRAttribute : ValidationAttribute
    {
        public UDPhoneNumberTRAttribute() { }
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var phoneTR = value.ToStringOrEmpty();
            if (phoneTR == "" && !validationContext.IsRequiredAttribute())
            {
                validationContext.SetValidatePropertyValue(null);
                return ValidationResult.Success;
            }
            if (TryValidators.TryPhoneNumberTR(phoneTR, out string _phonetr))
            {
                validationContext.SetValidatePropertyValue(_phonetr);
                return ValidationResult.Success;
            }
            if (this.ErrorMessage.IsNullOrEmpty())
            {
                this.ErrorMessage = $"{validationContext.DisplayName}, (xxx) xxx-xxxx biçimine uygun telefon numarası olmalıdır!";
                if (ValidationChecks.IsEnglishDefaultThreadCurrentUICulture) { this.ErrorMessage = $"{validationContext.DisplayName} must be a valid phone number in the format of (xxx) xxx-xxxx!"; }
            }
            return new(this.ErrorMessage, [validationContext.MemberName]);
        }
    }
}