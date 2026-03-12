namespace UD.Core.Attributes.DataAnnotations
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using UD.Core.Extensions;
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Parameter, AllowMultiple = false)]
    public sealed class UDEnumDataTypeAttribute<TEnum> : ValidationAttribute where TEnum : Enum
    {
        public bool CheckIsDefined { get; }
        public UDEnumDataTypeAttribute() : this(true) { }
        public UDEnumDataTypeAttribute(bool CheckIsDefined)
        {
            this.CheckIsDefined = CheckIsDefined;
        }
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null && !validationContext.IsRequiredAttribute()) { return ValidationResult.Success; }
            var typeofEnum = typeof(TEnum);
            if (value is TEnum _enum && (!this.CheckIsDefined || Enum.IsDefined(typeofEnum, _enum)))
            {
                validationContext.SetValidatePropertyValue(_enum);
                return ValidationResult.Success;
            }
            return new(this.ErrorMessage.CoalesceOrDefault($"{validationContext.DisplayName}, {typeofEnum.Name} türünden bir değer olmalıdır!"), [validationContext.MemberName]);
        }
    }
}