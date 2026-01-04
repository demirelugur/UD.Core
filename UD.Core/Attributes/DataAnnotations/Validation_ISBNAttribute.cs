namespace UD.Core.Attributes.DataAnnotations
{
    using UD.Core.Extensions;
    using UD.Core.Helper;
    using System.ComponentModel.DataAnnotations;
    using static UD.Core.Helper.GlobalConstants;
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Parameter, AllowMultiple = false)]
    public sealed class Validation_ISBNAttribute : ValidationAttribute
    {
        public Validation_ISBNAttribute() { }
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var _isbn = value.ToStringOrEmpty().ToUpper();
            if (ISBNHelper.IsValid(_isbn))
            {
                validationContext.SetValidatePropertyValue(_isbn);
                return ValidationResult.Success;
            }
            if (_isbn == "" && !validationContext.IsRequiredAttribute())
            {
                validationContext.SetValidatePropertyValue(null);
                return ValidationResult.Success;
            }
            return new(this.ErrorMessage.CoalesceOrDefault($"{validationContext.DisplayName}, {_title.isbn} biçimine uygun olmalıdır!"), new string[] { validationContext.MemberName });
        }
    }
}