using System.ComponentModel.DataAnnotations;
using UD.Core.Extensions;
using UD.Core.Helper;
namespace UD.Core.Attributes.DataAnnotations
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Parameter, AllowMultiple = false)]
    public sealed class UDUriAttribute : ValidationAttribute
    {
        public UDUriAttribute() { }
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var valueString = value.ToStringOrEmpty();
            if (valueString == "" && !validationContext.IsRequiredAttribute())
            {
                validationContext.SetValidatePropertyValue(null);
                return ValidationResult.Success;
            }
            if (TryValidators.TryUri(valueString, out var _uri))
            {
                validationContext.SetValidatePropertyValue(_uri.ToString().TrimEnd('/'));
                return ValidationResult.Success;
            }
            if (this.ErrorMessage.IsNullOrEmpty()) { this.ErrorMessage = $"{validationContext.DisplayName}, geçerli bir \"http, https\" protokollerine uygun {nameof(Uri)} adresi olmalıdır!"; }
            return new(this.ErrorMessage, [validationContext.MemberName]);
        }
    }
}