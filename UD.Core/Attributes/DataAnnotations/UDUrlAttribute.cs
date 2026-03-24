namespace UD.Core.Attributes.DataAnnotations
{
    using System.ComponentModel.DataAnnotations;
    using UD.Core.Extensions;
    using UD.Core.Helper;
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Parameter, AllowMultiple = false)]
    public sealed class UDUrlAttribute : ValidationAttribute
    {
        public UDUrlAttribute() { }
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var valueString = value.ToStringOrEmpty();
            if (valueString == "" && !validationContext.IsRequiredAttribute())
            {
                validationContext.SetValidatePropertyValue(null);
                return ValidationResult.Success;
            }
            if (Validators.TryUri(valueString, out Uri _uri))
            {
                validationContext.SetValidatePropertyValue(_uri.ToString().TrimEnd('/'));
                return ValidationResult.Success;
            }
            if (this.ErrorMessage.IsNullOrEmpty())
            {
                this.ErrorMessage = $"{validationContext.DisplayName}, geçerli bir \"http, https\" protokollerine uygun {nameof(Uri)} adresi olmalıdır!";
                if (Guards.IsEnglishDefaultThreadCurrentUICulture) { this.ErrorMessage = $"{validationContext.DisplayName} must be a valid {nameof(Uri)} address in the \"http, https\" protocols!"; }
            }
            return new(this.ErrorMessage, [validationContext.MemberName]);
        }
    }
}