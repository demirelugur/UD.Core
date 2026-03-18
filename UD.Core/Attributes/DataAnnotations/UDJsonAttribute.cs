namespace UD.Core.Attributes.DataAnnotations
{
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;
    using System.ComponentModel.DataAnnotations;
    using UD.Core.Extensions;
    using static UD.Core.Helper.OrtakTools;
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Parameter, AllowMultiple = false)]
    public sealed class UDJsonAttribute : ValidationAttribute
    {
        public JTokenType jTokenType { get; }
        public UDJsonAttribute(JTokenType jTokenType) { this.jTokenType = jTokenType; }
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var valueString = value.ToStringOrEmpty();
            var r = validationContext.IsRequiredAttribute();
            if (valueString == "" && !r)
            {
                validationContext.SetValidatePropertyValue(null);
                return ValidationResult.Success;
            }
            if (Validators.TryJson(valueString, this.jTokenType, out JToken _jToken))
            {
                if (_jToken.HasValues)
                {
                    validationContext.SetValidatePropertyValue(_jToken.ToString(Formatting.None));
                    return ValidationResult.Success;
                }
                if (!r)
                {
                    validationContext.SetValidatePropertyValue(null);
                    return ValidationResult.Success;
                }
            }
            if (this.ErrorMessage.IsNullOrEmpty())
            {
                this.ErrorMessage = $"{validationContext.DisplayName}, JSON biçimine ({this.jTokenType:g}) uygun olmalıdır!";
                if (Guards.IsEnglishDefaultThreadCurrentUICulture) { this.ErrorMessage = $"{validationContext.DisplayName} must be in JSON format ({this.jTokenType:g})!"; }
            }
            return new(this.ErrorMessage, [validationContext.MemberName]);
        }
    }
}