using System.ComponentModel.DataAnnotations;
using UD.Core.Extensions;
namespace UD.Core.Attributes.DataAnnotations
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Parameter, AllowMultiple = false)]
    public sealed class UDStringIncludesAttribute : ValidationAttribute
    {
        public bool IsEqual { get; }
        public StringComparison Comparison { get; }
        public string[] Values { get; }
        public UDStringIncludesAttribute(bool isEqual, params string[] values) : this(isEqual, StringComparison.Ordinal, values) { }
        public UDStringIncludesAttribute(bool isEqual, StringComparison comparison, params string[] values)
        {
            this.IsEqual = isEqual;
            this.Comparison = comparison;
            this.Values = values ?? [];
        }
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var isRequired = validationContext.IsRequiredAttribute();
            if (value == null)
            {
                if (isRequired) { return this.ToResult(validationContext); }
                return ValidationResult.Success;
            }
            if (value is not String _s) { return new($"{validationContext.DisplayName} alanı geçersiz bir türde.", [validationContext.MemberName]); }
            _s = _s.Trim();
            var contains = false;
            if (_s.IsNullOrEmpty())
            {
                if (isRequired) { contains = false; } // Zorunlu alanda boş string geçersiz sayılacak
                else
                {
                    validationContext.SetValidatePropertyValue(null);
                    return ValidationResult.Success;
                }
            }
            else { contains = this.Values.Any(x => String.Equals(x, _s, this.Comparison)); }
            return (contains == this.IsEqual ? ValidationResult.Success : this.ToResult(validationContext));
        }
        private ValidationResult ToResult(ValidationContext validationContext)
        {
            if (this.ErrorMessage.IsNullOrEmpty())
            {
                var valuesStr = String.Join(", ", this.Values);
                if (this.IsEqual) { this.ErrorMessage = $"{validationContext.DisplayName}, [{valuesStr}] değerlerinden biri olmalıdır!"; }
                else { this.ErrorMessage = $"{validationContext.DisplayName}, [{valuesStr}] değerleri dışında farklı bir değer olmalıdır!"; }
            }
            return new(this.ErrorMessage, [validationContext.MemberName]);
        }
    }
}