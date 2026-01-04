namespace UD.Core.Attributes.DataAnnotations
{
    using System.ComponentModel.DataAnnotations;
    using UD.Core.Extensions;
    using static UD.Core.Helper.OrtakTools;
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Parameter, AllowMultiple = false)]
    public sealed class Validation_UrlHttpAttribute : ValidationAttribute
    {
        public Validation_UrlHttpAttribute() { }
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var uri = value.ToStringOrEmpty();
            if (_try.TryUri(uri, out Uri _uri))
            {
                validationContext.SetValidatePropertyValue(_uri.ToString().TrimEnd('/'));
                return ValidationResult.Success;
            }
            if (uri == "" && !validationContext.IsRequiredAttribute())
            {
                validationContext.SetValidatePropertyValue(null);
                return ValidationResult.Success;
            }
            return new(this.ErrorMessage.CoalesceOrDefault($"{validationContext.DisplayName}, geçerli bir \"http, https\" protokollerine uygun {nameof(Uri)} adresi olmalıdır!"), new string[] { validationContext.MemberName });
        }
    }
}