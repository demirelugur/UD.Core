namespace UD.Core.Attributes.DataAnnotations
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using UD.Core.Extensions;
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
            return new(this.ErrorMessage.CoalesceOrDefault($"{validationContext.DisplayName}, T.C. Vergi Kimlik Numarası biçimine uygun olmalıdır!"), [validationContext.MemberName]);
        }
    }
}