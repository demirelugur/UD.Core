namespace UD.Core.Attributes.DataAnnotations
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using UD.Core.Extensions;
    using UD.Core.Helper;
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Parameter, AllowMultiple = false)]
    public sealed class UDIncludesAttribute : ValidationAttribute
    {
        public bool IsEqual { get; }
        public object[] Values { get; }
        public UDIncludesAttribute(bool isEqual, params object[] values)
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
            bool contains;
            if (value is String _s)
            {
                _s = _s.Trim();
                if (_s == "")
                {
                    if (isRequired) { contains = false; }
                    else
                    {
                        validationContext.SetValidatePropertyValue(null);
                        return ValidationResult.Success;
                    }
                }
                else { contains = this.Values.Select(x => x.ToString()).Contains(_s); }
            }
            else if (value is (Byte or Int16 or Int32 or Int64))
            {
                var valueLong = Convert.ToInt64(value);
                contains = this.Values.Any(v => Convert.ToInt64(v) == valueLong);
            }
            else { contains = this.Values.Any(v => v.ToString() == value.ToString()); }
            return (contains == this.IsEqual ? ValidationResult.Success : this.ToResult(validationContext));
        }
        private ValidationResult ToResult(ValidationContext validationContext)
        {
            if (this.ErrorMessage.IsNullOrEmpty())
            {
                if (this.IsEqual)
                {
                    this.ErrorMessage = $"{validationContext.DisplayName}, [{String.Join(", ", this.Values)}] değerlerinden biri olmalıdır!";
                    if (Checks.IsEnglishCurrentUICulture) { this.ErrorMessage = $"{validationContext.DisplayName} must be one of the values [{String.Join(", ", this.Values)}]!"; }
                }
                else
                {
                    this.ErrorMessage = $"{validationContext.DisplayName}, [{String.Join(", ", this.Values)}] değerleri dışında farklı bir değer olmalıdır!";
                    if (Checks.IsEnglishCurrentUICulture) { this.ErrorMessage = $"{validationContext.DisplayName} must be a different value than [{String.Join(", ", this.Values)}]!"; }
                }
            }
            return new(this.ErrorMessage, [validationContext.MemberName]);
        }
    }
}