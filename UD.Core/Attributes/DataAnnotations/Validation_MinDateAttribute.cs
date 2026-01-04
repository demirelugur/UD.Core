namespace UD.Core.Attributes.DataAnnotations
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using UD.Core.Extensions;
    using UD.Core.Helper;
    using static UD.Core.Helper.GlobalConstants;
    using static UD.Core.Helper.OrtakTools;
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Parameter, AllowMultiple = false)]
    public class Validation_MinDateAttribute : ValidationAttribute
    {
        public object mindate { get; set; }
        public Validation_MinDateAttribute() : this(1970, 1, 1) { }
        public Validation_MinDateAttribute(int yil, int ay, int gun) : this(new DateOnly(yil, ay, gun)) { }
        public Validation_MinDateAttribute(object mindate)
        {
            Guard.CheckNull(mindate, nameof(mindate));
            this.mindate = mindate;
        }
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null && !validationContext.IsRequiredAttribute())
            {
                validationContext.SetValidatePropertyValue(null);
                return ValidationResult.Success;
            }
            var _d = _to.ToDateOnlyFromObject(this.mindate);
            if (value is DateOnly _dateonly && _dateonly >= _d)
            {
                validationContext.SetValidatePropertyValue(_dateonly);
                return ValidationResult.Success;
            }
            if (value is DateTime _datetime && _datetime.ToDateOnly() >= _d)
            {
                validationContext.SetValidatePropertyValue(_datetime);
                return ValidationResult.Success;
            }
            return new(this.ErrorMessage ?? $"{validationContext.DisplayName}, {_d.ToString(_date.ddMMyyyy)} tarihinden ileri bir tarih olmalıdır!", new string[] { validationContext.MemberName });
        }
    }
}