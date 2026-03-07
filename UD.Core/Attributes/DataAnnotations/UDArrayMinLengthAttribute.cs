namespace UD.Core.Attributes.DataAnnotations
{
    using System.ComponentModel.DataAnnotations;
    using static UD.Core.Helper.GlobalConstants;
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Parameter, AllowMultiple = false)]
    public sealed class UDArrayMinLengthAttribute : MinLengthAttribute
    {
        public UDArrayMinLengthAttribute() : this(1) { }
        public UDArrayMinLengthAttribute(int minimumLength) : base(minimumLength)
        {
            this.ErrorMessage = minimumLength > 1 ? ValidationErrorMessageConstants.ArrayMinLength : ValidationErrorMessageConstants.Required;
        }
    }
}