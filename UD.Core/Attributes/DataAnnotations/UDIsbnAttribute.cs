namespace UD.Core.Attributes.DataAnnotations
{
    using System.ComponentModel.DataAnnotations;
    using UD.Core.Extensions;
    using UD.Core.Helper.Configuration;
    using static UD.Core.Helper.GlobalConstants;
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Parameter, AllowMultiple = false)]
    public sealed class UDIsbnAttribute : ValidationAttribute
    {
        public UDIsbnAttribute() { }
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var isbn = value.ToStringOrEmpty().ToUpper();
            if (isbn == "" && !validationContext.IsRequiredAttribute())
            {
                validationContext.SetValidatePropertyValue(null);
                return ValidationResult.Success;
            }
            if (ISBNHelper.IsValid(isbn))
            {
                validationContext.SetValidatePropertyValue(isbn);
                return ValidationResult.Success;
            }
            return new(this.ErrorMessage.CoalesceOrDefault($"{validationContext.DisplayName}, {TitleConstants.Isbn} biçimine uygun olmalıdır!"), new[] { validationContext.MemberName });
        }
    }
}