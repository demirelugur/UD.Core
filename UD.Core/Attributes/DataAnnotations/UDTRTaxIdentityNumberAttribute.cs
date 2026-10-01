using System;
using System.ComponentModel.DataAnnotations;
using UD.Core.Extensions;
namespace UD.Core.Attributes.DataAnnotations
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Parameter, AllowMultiple = false)]
    public sealed class UDTRTaxIdentityNumberAttribute : ValidationAttribute
    {
        public UDTRTaxIdentityNumberAttribute() { }
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null && !validationContext.IsRequiredAttribute()) { return ValidationResult.Success; }
            var valueLong = value.ToLong();
            if (valueLong.IsTRTaxIdentityNumber())
            {
                validationContext.SetValidatePropertyValue(valueLong);
                return ValidationResult.Success;
            }
            if (this.ErrorMessage.IsNullOrEmpty()) { this.ErrorMessage = $"{validationContext.DisplayName}, T.C. Vergi Kimlik Numarası biçimine uygun olmalıdır!"; }
            return new(this.ErrorMessage, [validationContext.MemberName]);
        }
    }
}