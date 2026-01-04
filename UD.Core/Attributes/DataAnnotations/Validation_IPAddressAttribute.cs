namespace UD.Core.Attributes.DataAnnotations
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Net;
    using UD.Core.Extensions;
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Parameter, AllowMultiple = false)]
    public sealed class Validation_IPAddressAttribute : ValidationAttribute
    {
        public Validation_IPAddressAttribute() { }
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var ip = value.ToStringOrEmpty();
            if (IPAddress.TryParse(ip, out IPAddress _ip))
            {
                validationContext.SetValidatePropertyValue(_ip.MapToIPv4().ToString());
                return ValidationResult.Success;
            }
            if (ip == "" && !validationContext.IsRequiredAttribute())
            {
                validationContext.SetValidatePropertyValue(null);
                return ValidationResult.Success;
            }
            return new(this.ErrorMessage.CoalesceOrDefault($"{validationContext.DisplayName}, geçerli bir IP adresi olmalıdır!"), new string[] { validationContext.MemberName });
        }
    }
}