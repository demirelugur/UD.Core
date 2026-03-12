namespace UD.Core.Attributes.DataAnnotations
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Net;
    using UD.Core.Extensions;
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
            return new(this.ErrorMessage.CoalesceOrDefault($"{validationContext.DisplayName}, geçerli bir IP adresi olmalıdır!"), [validationContext.MemberName]);
        }
    }
}