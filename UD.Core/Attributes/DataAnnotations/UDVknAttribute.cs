namespace UD.Core.Attributes.DataAnnotations
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using UD.Core.Extensions;
    using static UD.Core.Helper.OrtakTools;
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Parameter, AllowMultiple = false)]
    public sealed class UDVknAttribute : ValidationAttribute
    {
        public UDVknAttribute() { }
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null && !validationContext.IsRequiredAttribute()) { return ValidationResult.Success; }
            var valueLong = value.ToLong();
            if (valueLong.IsVergiKimlikNo())
            {
                validationContext.SetValidatePropertyValue(valueLong);
                return ValidationResult.Success;
            }
            if (this.ErrorMessage.IsNullOrEmpty())
            {
                this.ErrorMessage = $"{validationContext.DisplayName}, T.C. Vergi Kimlik Numarası biçimine uygun olmalıdır!";
                if (Guards.IsUICultureEnglish) { this.ErrorMessage = $"{validationContext.DisplayName} must be in a valid T.C. Tax Identity Number format!"; }
            }
            return new(this.ErrorMessage, [validationContext.MemberName]);
        }
    }
}