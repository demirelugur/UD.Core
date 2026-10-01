using System.ComponentModel.DataAnnotations;
using UD.Core.Extensions;
namespace UD.Core.Attributes.DataAnnotations
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Parameter, AllowMultiple = false)]
    public sealed class UDEnumAttribute<TEnum> : ValidationAttribute where TEnum : struct, Enum
    {
        public bool CheckIsDefined { get; }
        public UDEnumAttribute() : this(true) { }
        public UDEnumAttribute(bool checkIsDefined)
        {
            this.CheckIsDefined = checkIsDefined;
        }
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null && !validationContext.IsRequiredAttribute()) { return ValidationResult.Success; }
            var enumValue = value.TryToEnum<TEnum>();
            var typeofEnum = typeof(TEnum);
            if (enumValue.HasValue && (!this.CheckIsDefined || Enum.IsDefined(typeofEnum, enumValue.Value))) { return ValidationResult.Success; }
            if (this.ErrorMessage.IsNullOrEmpty()) { this.ErrorMessage = $"{validationContext.DisplayName}, {typeofEnum.FullName} türünden bir {nameof(Enum)} değeri olmalıdır!"; }
            return new(this.ErrorMessage, [validationContext.MemberName]);
        }
    }
}