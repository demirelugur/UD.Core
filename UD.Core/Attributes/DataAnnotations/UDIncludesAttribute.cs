using System.ComponentModel.DataAnnotations;
using UD.Core.Extensions;
namespace UD.Core.Attributes.DataAnnotations
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Parameter, AllowMultiple = false)]
    public sealed class UDIncludesAttribute<T> : ValidationAttribute where T : struct
    {
        public bool IsEqual { get; }
        public T[] Values { get; }
        public UDIncludesAttribute(bool isEqual, params T[] values)
        {
            this.IsEqual = isEqual;
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
            if (value is not T _typedValue) { return new($"{validationContext.DisplayName} alanı uygun türde değil.", [validationContext.MemberName]); }
            var contains = false;
            var type = typeof(T);
            if (type.IsEnum || type.Includes(typeof(byte), typeof(short), typeof(int), typeof(long)))
            {
                var valueLong = Convert.ToInt64(_typedValue);
                contains = this.Values.Any(v => Convert.ToInt64(v) == valueLong);
            }
            else if (_typedValue is DateTime dt) { contains = this.Values.Cast<DateTime>().Any(v => v == dt); }
            else { contains = this.Values.Any(v => v.Equals(_typedValue)); }
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