namespace UD.Core.Attributes.DataAnnotations
{
    using System.ComponentModel.DataAnnotations;
    using UD.Core.Extensions;
    using static UD.Core.Helper.OrtakTools;
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
            if (Validators.TryPhoneNumberTR(phoneTR, out string _phonetr))
            {
                validationContext.SetValidatePropertyValue(_phonetr);
                return ValidationResult.Success;
            }
            return new(this.ErrorMessage.CoalesceOrDefault($"{validationContext.DisplayName}, (xxx) xxx-xxxx biçimine uygun telefon numarası olmalıdır!"), new[] { validationContext.MemberName });
        }
    }
}