namespace UD.Core.Attributes.DataAnnotations
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using UD.Core.Extensions;
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Parameter, AllowMultiple = false)]
    public sealed class Validation_IncludesAttribute : ValidationAttribute
    {
        public bool isequal { get; set; }
        public object[] values { get; set; }
        public Validation_IncludesAttribute(bool isequal, params object[] values)
        {
            this.isequal = isequal;
            this.values = values ?? Array.Empty<object>();
        }
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var _isrequired = validationContext.IsRequiredAttribute();
            if (value == null)
            {
                if (_isrequired) { return this.tovalidationresult(validationContext); }
                return ValidationResult.Success;
            }
            bool _contains;
            if (value is String _s)
            {
                _s = _s.Trim();
                if (_s == "")
                {
                    if (_isrequired) { _contains = false; }
                    else
                    {
                        validationContext.SetValidatePropertyValue(null);
                        return ValidationResult.Success;
                    }
                }
                else { _contains = this.values.Select(x => x.ToString()).Contains(_s); }
            }
            else if (value is (Byte or Int16 or Int32 or Int64))
            {
                var _longval = Convert.ToInt64(value);
                _contains = this.values.Any(v => Convert.ToInt64(v) == _longval);
            }
            else { _contains = this.values.Any(v => v.ToString() == value.ToString()); }
            if (_contains == this.isequal) { return ValidationResult.Success; }
            return this.tovalidationresult(validationContext);
        }
        private ValidationResult tovalidationresult(ValidationContext validationContext)
        {
            var _r = this.ErrorMessage.ToStringOrEmpty();
            if (_r == "")
            {
                if (this.isequal) { _r = $"{validationContext.DisplayName}, [{String.Join(", ", this.values)}] değerlerinden biri olmalıdır!"; }
                else { _r = $"{validationContext.DisplayName}, [{String.Join(", ", this.values)}] değerleri dışında farklı bir değer olmalıdır!"; }
            }
            return new(_r, new string[] { validationContext.MemberName });
        }
    }
}