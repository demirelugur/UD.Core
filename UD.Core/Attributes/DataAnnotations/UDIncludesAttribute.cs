namespace UD.Core.Attributes.DataAnnotations
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using UD.Core.Extensions;
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Parameter, AllowMultiple = false)]
    public sealed class UDIncludesAttribute : ValidationAttribute
    {
        public bool isequal { get; }
        public object[] values { get; }
        public UDIncludesAttribute(bool isequal, params object[] values)
        {
            this.isequal = isequal;
            this.values = values ?? [];
        }
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var isRequired = validationContext.IsRequiredAttribute();
            if (value == null)
            {
                if (isRequired) { return this.tovalidationresult(validationContext); }
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
                else { contains = this.values.Select(x => x.ToString()).Contains(_s); }
            }
            else if (value is (Byte or Int16 or Int32 or Int64))
            {
                var valueLong = Convert.ToInt64(value);
                contains = this.values.Any(v => Convert.ToInt64(v) == valueLong);
            }
            else { contains = this.values.Any(v => v.ToString() == value.ToString()); }
            if (contains == this.isequal) { return ValidationResult.Success; }
            return this.tovalidationresult(validationContext);
        }
        private ValidationResult tovalidationresult(ValidationContext validationContext)
        {
            var r = this.ErrorMessage.ToStringOrEmpty();
            if (r == "")
            {
                if (this.isequal) { r = $"{validationContext.DisplayName}, [{String.Join(", ", this.values)}] değerlerinden biri olmalıdır!"; }
                else { r = $"{validationContext.DisplayName}, [{String.Join(", ", this.values)}] değerleri dışında farklı bir değer olmalıdır!"; }
            }
            return new(r, new[] { validationContext.MemberName });
        }
    }
}