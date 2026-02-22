namespace UD.Core.Attributes.DataAnnotations
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using UD.Core.Extensions;
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Parameter, AllowMultiple = false)]
    public sealed class Validation_VknAttribute : ValidationAttribute
    {
        public Validation_VknAttribute() { }
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null && !validationContext.IsRequiredAttribute()) { return ValidationResult.Success; }
            var _l = value.ToLong();
            if (_l.IsVergiKimlikNo())
            {
                validationContext.SetValidatePropertyValue(_l);
                return ValidationResult.Success;
            }
            return new(this.ErrorMessage.CoalesceOrDefault($"{validationContext.DisplayName}, T.C. Vergi Kimlik Numarası biçimine uygun olmalıdır!"), new string[] { validationContext.MemberName });
        }
    }
}