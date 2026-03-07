namespace UD.Core.Attributes.DataAnnotations
{
    using System.ComponentModel.DataAnnotations;
    using UD.Core.Extensions;
    using static UD.Core.Helper.GlobalConstants;
    using static UD.Core.Helper.OrtakTools;
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Parameter, AllowMultiple = false)]
    public sealed class Validation_MACAddressAttribute : ValidationAttribute
    {
        public Validation_MACAddressAttribute() { }
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var valueString = value.ToStringOrEmpty();
            if (valueString == "" && !validationContext.IsRequiredAttribute())
            {
                validationContext.SetValidatePropertyValue(null);
                return ValidationResult.Success;
            }
            if (Validators.TryMACAddress(valueString, out string _mac))
            {
                validationContext.SetValidatePropertyValue(_mac);
                return ValidationResult.Success;
            }
            return new(this.ErrorMessage.CoalesceOrDefault($"{validationContext.DisplayName}, {TitleConstants.MAC} Adresi biçimine uygun olmalıdır!"), new string[] { validationContext.MemberName });
        }
    }
}