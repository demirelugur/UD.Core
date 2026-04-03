namespace UD.Core.Attributes.DataAnnotations
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Net;
    using UD.Core.Extensions;
    using UD.Core.Extensions.Common;
    using UD.Core.Helper;
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Parameter, AllowMultiple = false)]
    public sealed class UDIPAddressAttribute : ValidationAttribute
    {
        public UDIPAddressAttribute() { }
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var ip = value.ToStringOrEmpty();
            if (ip == "" && !validationContext.IsRequiredAttribute())
            {
                validationContext.SetValidatePropertyValue(null);
                return ValidationResult.Success;
            }
            if (IPAddress.TryParse(ip, out IPAddress _ip))
            {
                validationContext.SetValidatePropertyValue(_ip.MapToIPv4().ToString());
                return ValidationResult.Success;
            }
            if (this.ErrorMessage.IsNullOrEmpty())
            {
                this.ErrorMessage = $"{validationContext.DisplayName}, geçerli bir IP adresi olmalıdır!";
                if (ValidationChecks.IsEnglishDefaultThreadCurrentUICulture) { this.ErrorMessage = $"{validationContext.DisplayName} must be a valid IP address!"; }
            }
            return new(this.ErrorMessage, [validationContext.MemberName]);
        }
    }
}