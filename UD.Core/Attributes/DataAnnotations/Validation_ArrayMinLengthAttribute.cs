namespace UD.Core.Attributes.DataAnnotations
{
    using System.ComponentModel.DataAnnotations;
    using static UD.Core.Helper.GlobalConstants;
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Parameter, AllowMultiple = false)]
    public sealed class Validation_ArrayMinLengthAttribute : MinLengthAttribute
    {
        public Validation_ArrayMinLengthAttribute() : this(1) { }
        public Validation_ArrayMinLengthAttribute(int minimumLength) : base(minimumLength)
        {
            this.ErrorMessage = minimumLength > 1 ? ValidationErrorMessageConstants.ArrayMinLength : ValidationErrorMessageConstants.Required;
        }
    }
}