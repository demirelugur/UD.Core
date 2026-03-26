namespace UD.Core.Attributes.DataAnnotations
{
    using System.ComponentModel.DataAnnotations;
    using UD.Core.Helper;
    using static UD.Core.Helper.GlobalConstants;
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Parameter, AllowMultiple = false)]
    public sealed class UDArrayMaxLengthAttribute : MaxLengthAttribute
    {
        public UDArrayMaxLengthAttribute(int maximumLength) : base(maximumLength)
        {
            this.ErrorMessage = Guards.IsEnglishDefaultThreadCurrentUICulture ? "{0} cannot contain more than {1} elements." : ValidationErrorMessageConstants.ArrayMaxLength;
        }
    }
}