using System.ComponentModel.DataAnnotations;
using static UD.Core.Helper.GlobalConstants;
namespace UD.Core.Attributes.DataAnnotations
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Parameter, AllowMultiple = false)]
    public sealed class UDStringLengthAttribute : StringLengthAttribute
    {
        public UDStringLengthAttribute(int maximumlength) : base(maximumlength)
        {
            this.ErrorMessage = ValidationMessageTurkishConstants.StringLengthMax;
        }
        public UDStringLengthAttribute(int maximumlength, int minimumlength) : base(maximumlength)
        {
            this.MinimumLength = minimumlength;
            this.ErrorMessage = (maximumlength == minimumlength ? ValidationMessageTurkishConstants.StringLengthEqualMaxMin : ValidationMessageTurkishConstants.StringLengthBetweenMaxMin);
        }
    }
}