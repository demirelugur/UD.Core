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
            this.values = values ?? [];
        }
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var isrequired = validationContext.IsRequiredAttribute();
            if (value == null)
            {
                if (isrequired) { return this.tovalidationresult(validationContext); }
                return ValidationResult.Success;
            }
            bool contains;
            if (value is String _s)
            {
                _s = _s.Trim();
                if (_s == "")
                {
                    if (isrequired) { contains = false; }
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
                var longval = Convert.ToInt64(value);
                contains = this.values.Any(v => Convert.ToInt64(v) == longval);
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
            return new(r, new string[] { validationContext.MemberName });
        }
    }
}