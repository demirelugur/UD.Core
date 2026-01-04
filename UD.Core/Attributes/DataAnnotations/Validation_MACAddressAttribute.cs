namespace UD.Core.Attributes.DataAnnotations
{
    using UD.Core.Extensions;
    using System.ComponentModel.DataAnnotations;
    using static UD.Core.Helper.GlobalConstants;
    using static UD.Core.Helper.OrtakTools;
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Parameter, AllowMultiple = false)]
    public sealed class Validation_MACAddressAttribute : ValidationAttribute
    {
        public Validation_MACAddressAttribute() { }
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var mac = value.ToStringOrEmpty();
            if (_try.TryMACAddress(mac, out string _mac))
            {
                validationContext.SetValidatePropertyValue(_mac);
                return ValidationResult.Success;
            }
            if (mac == "" && !validationContext.IsRequiredAttribute())
            {
                validationContext.SetValidatePropertyValue(null);
                return ValidationResult.Success;
            }
            return new(this.ErrorMessage.CoalesceOrDefault($"{validationContext.DisplayName}, {_title.mac} Adresi biçimine uygun olmalıdır!"), new string[] { validationContext.MemberName });
        }
    }
}