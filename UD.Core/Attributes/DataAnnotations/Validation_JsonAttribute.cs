namespace UD.Core.Attributes.DataAnnotations
{
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;
    using System.ComponentModel.DataAnnotations;
    using UD.Core.Extensions;
    using static UD.Core.Helper.OrtakTools;
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Parameter, AllowMultiple = false)]
    public sealed class Validation_JsonAttribute : ValidationAttribute
    {
        public JTokenType jtokentype { get; set; }
        public Validation_JsonAttribute(JTokenType jtokentype) { this.jtokentype = jtokentype; }
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var jsondata = value.ToStringOrEmpty();
            var r = validationContext.IsRequiredAttribute();
            if (Validators.TryJson(jsondata, this.jtokentype, out JToken _jt))
            {
                if (_jt.HasValues)
                {
                    validationContext.SetValidatePropertyValue(_jt.ToString(Formatting.None));
                    return ValidationResult.Success;
                }
                if (!r)
                {
                    validationContext.SetValidatePropertyValue(null);
                    return ValidationResult.Success;
                }
            }
            if (jsondata == "" && !r)
            {
                validationContext.SetValidatePropertyValue(null);
                return ValidationResult.Success;
            }
            return new(this.ErrorMessage.CoalesceOrDefault($"{validationContext.DisplayName}, JSON biçimine ({this.jtokentype.ToString("g")}) uygun olmalıdır!"), new string[] { validationContext.MemberName });
        }
    }
}