using System.ComponentModel.DataAnnotations;
using UD.Core.Extensions;
using UD.Core.Helper;
using static UD.Core.Helper.GlobalConstants;
namespace UD.Core.Attributes.DataAnnotations
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Parameter, AllowMultiple = false)]
    public sealed class UDISBNAttribute : ValidationAttribute
    {
        public UDISBNAttribute() { }
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
            if (this.ErrorMessage.IsNullOrEmpty()) { this.ErrorMessage = $"{validationContext.DisplayName}, {TitleConstants.Isbn} biçimine uygun olmalıdır!"; }
            return new(this.ErrorMessage, [validationContext.MemberName]);
        }
    }
}