using System.ComponentModel.DataAnnotations;
using UD.Core.Extensions;
using UD.Core.Helper;
namespace UD.Core.Attributes.DataAnnotations
{
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
            if (TryValidators.TryPhoneNumberTR(phoneTR, out var _phoneTR))
            {
                validationContext.SetValidatePropertyValue(_phoneTR);
                return ValidationResult.Success;
            }
            if (this.ErrorMessage.IsNullOrEmpty()) { this.ErrorMessage = $"{validationContext.DisplayName}, (xxx) xxx-xxxx biçimine uygun telefon numarası olmalıdır!"; }
            return new(this.ErrorMessage, [validationContext.MemberName]);
        }
    }
}