namespace UD.Core.Attributes.DataAnnotations
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using UD.Core.Extensions;
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Parameter, AllowMultiple = false)]
    public sealed class Validation_TcknAttribute : ValidationAttribute
    {
        public Validation_TcknAttribute() { }
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null && !validationContext.IsRequiredAttribute()) { return ValidationResult.Success; }
            var valueLong = value.ToLong();
            if (valueLong.IsTCKimlikNo())
            {
                validationContext.SetValidatePropertyValue(valueLong);
                return ValidationResult.Success;
            }
            return new(this.ErrorMessage.CoalesceOrDefault($"{validationContext.DisplayName}, T.C. Kimlik Numarası biçimine uygun olmalıdır!"), new string[] { validationContext.MemberName });
        }
    }
}