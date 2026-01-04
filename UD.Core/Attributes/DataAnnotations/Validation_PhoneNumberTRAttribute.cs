namespace UD.Core.Attributes.DataAnnotations
{
    using System.ComponentModel.DataAnnotations;
    using UD.Core.Extensions;
    using static UD.Core.Helper.OrtakTools;
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Parameter, AllowMultiple = false)]
    public sealed class Validation_PhoneNumberTRAttribute : ValidationAttribute
    {
        public Validation_PhoneNumberTRAttribute() { }
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var phoneTR = value.ToStringOrEmpty();
            if (_try.TryPhoneNumberTR(phoneTR, out string _phonetr))
            {
                validationContext.SetValidatePropertyValue(_phonetr);
                return ValidationResult.Success;
            }
            if (phoneTR == "" && !validationContext.IsRequiredAttribute())
            {
                validationContext.SetValidatePropertyValue(null);
                return ValidationResult.Success;
            }
            return new(this.ErrorMessage.CoalesceOrDefault($"{validationContext.DisplayName}, (xxx) xxx-xxxx biçimine uygun telefon numarası olmalıdır!"), new string[] { validationContext.MemberName });
        }
    }
}