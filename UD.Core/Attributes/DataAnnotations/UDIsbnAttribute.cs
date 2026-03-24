namespace UD.Core.Attributes.DataAnnotations
{
    using System.ComponentModel.DataAnnotations;
    using UD.Core.Extensions;
    using UD.Core.Helper.Configuration;
    using static UD.Core.Helper.GlobalConstants;
    using UD.Core.Helper;
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
            if (this.ErrorMessage.IsNullOrEmpty())
            {
                this.ErrorMessage = $"{validationContext.DisplayName}, {TitleConstants.Isbn} biçimine uygun olmalıdır!";
                if(Guards.IsEnglishDefaultThreadCurrentUICulture) { this.ErrorMessage = $"{validationContext.DisplayName} must be in a valid {TitleConstants.Isbn} format!"; }
            }
            return new(this.ErrorMessage, [validationContext.MemberName]);
        }
    }
}