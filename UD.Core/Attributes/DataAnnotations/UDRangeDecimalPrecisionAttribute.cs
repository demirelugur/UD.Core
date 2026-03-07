namespace UD.Core.Attributes.DataAnnotations
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Globalization;
    using UD.Core.Extensions;
    using static UD.Core.Helper.GlobalConstants;
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Parameter, AllowMultiple = false)]
    public sealed class UDRangeDecimalPrecisionAttribute : RangeAttribute
    {
        public int Precision { get; }
        public int Scale { get; }
        public UDRangeDecimalPrecisionAttribute(object min, int precision, int scale) : base(typeof(decimal), Convert.ToString(min, CultureInfo.InvariantCulture), GetMaxString(precision, scale))
        {
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(precision);
            ArgumentOutOfRangeException.ThrowIfNegative(scale);
            if (scale > precision) { throw new ArgumentException($"{nameof(this.Scale)}, {nameof(this.Precision)}'dan büyük olamaz."); }
            this.Precision = precision;
            this.Scale = scale;
        }
        protected override ValidationResult? IsValid(object value, ValidationContext validationContext)
        {
            if (value == null && !validationContext.IsRequiredAttribute()) { return ValidationResult.Success; }
            var valueDecimal = value.ToDecimal();
            if (valueDecimal >= Convert.ToDecimal(this.Minimum) && valueDecimal <= Convert.ToDecimal(this.Maximum)) { return ValidationResult.Success; }
            return new(this.ErrorMessage.CoalesceOrDefault($"{String.Format(ValidationErrorMessageConstants.Range, validationContext.DisplayName, this.Minimum, this.Maximum)} ({nameof(this.Precision)}={this.Precision}, {nameof(this.Scale)}={this.Scale})."), new[] { validationContext.MemberName });
        }
        private static string GetMaxString(int precision, int scale)
        {
            var tenPowInt = Pow10(precision - scale);
            var tenPowScale = Pow10(scale);
            return (tenPowInt - (1m / tenPowScale)).ToString(CultureInfo.InvariantCulture);
        }
        private static decimal Pow10(int n)
        {
            int i;
            var result = 1m;
            for (i = 0; i < n; i++) { result *= 10m; }
            return result;
        }
    }
}