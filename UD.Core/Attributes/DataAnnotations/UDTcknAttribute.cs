namespace UD.Core.Attributes.DataAnnotations
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using UD.Core.Extensions;
    using UD.Core.Helper;
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Parameter, AllowMultiple = false)]
    public sealed class UDTcknAttribute : ValidationAttribute
    {
        public UDTcknAttribute() { }
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null && !validationContext.IsRequiredAttribute()) { return ValidationResult.Success; }
            var valueLong = value.ToLong();
            if (valueLong.IsTCKimlikNo())
            {
                validationContext.SetValidatePropertyValue(valueLong);
                return ValidationResult.Success;
            }
            if (this.ErrorMessage.IsNullOrEmpty())
            {
                this.ErrorMessage = $"{validationContext.DisplayName}, T.C. Kimlik Numarası biçimine uygun olmalıdır!";
                if (ValidationChecks.IsEnglishCurrentUICulture) { this.ErrorMessage = $"{validationContext.DisplayName} must be in a valid T.C. Identity Number format!"; }
            }
            return new(this.ErrorMessage, [validationContext.MemberName]);
        }
    }
}