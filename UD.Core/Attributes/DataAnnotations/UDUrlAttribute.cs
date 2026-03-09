namespace UD.Core.Attributes.DataAnnotations
{
    using System.ComponentModel.DataAnnotations;
    using UD.Core.Extensions;
    using static UD.Core.Helper.OrtakTools;
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
            return new(this.ErrorMessage.CoalesceOrDefault($"{validationContext.DisplayName}, geçerli bir \"http, https\" protokollerine uygun {nameof(Uri)} adresi olmalıdır!"), new[] { validationContext.MemberName });
        }
    }
}