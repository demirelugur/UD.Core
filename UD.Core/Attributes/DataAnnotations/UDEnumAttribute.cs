namespace UD.Core.Attributes.DataAnnotations
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using UD.Core.Extensions;
    using UD.Core.Helper;
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Parameter, AllowMultiple = false)]
    public sealed class UDEnumAttribute<TEnum> : ValidationAttribute where TEnum : struct, Enum
    {
        public bool checkIsDefined { get; }
        public UDEnumAttribute() : this(true) { }
        public UDEnumAttribute(bool checkIsDefined)
        {
            this.checkIsDefined = checkIsDefined;
        }
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null && !validationContext.IsRequiredAttribute()) { return ValidationResult.Success; }
            var enumValue = value.TryToEnum<TEnum>();
            var typeofEnum = typeof(TEnum);
            if (enumValue.HasValue && (!this.checkIsDefined || Enum.IsDefined(typeofEnum, enumValue.Value))) { return ValidationResult.Success; }
            if (this.ErrorMessage.IsNullOrEmpty())
            {
                this.ErrorMessage = $"{validationContext.DisplayName}, {typeofEnum.FullName} türünden bir {nameof(Enum)} değeri olmalıdır!";
                if (Guards.IsEnglishDefaultThreadCurrentUICulture) { this.ErrorMessage = $"{validationContext.DisplayName} must be a valid {nameof(Enum)} value of type {typeofEnum.FullName}!"; }
            }
            return new(this.ErrorMessage, [validationContext.MemberName]);
        }
    }
}