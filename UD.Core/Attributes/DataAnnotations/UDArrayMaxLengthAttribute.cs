namespace UD.Core.Attributes.DataAnnotations
{
    using System.ComponentModel.DataAnnotations;
    using static UD.Core.Helper.GlobalConstants;
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Parameter, AllowMultiple = false)]
    public sealed class UDArrayMaxLengthAttribute : MaxLengthAttribute
    {
        public UDArrayMaxLengthAttribute(int maximumLength) : base(maximumLength)
        {
            this.ErrorMessage = ValidationMessageTurkishConstants.ArrayMaxLength;
        }
    }
}