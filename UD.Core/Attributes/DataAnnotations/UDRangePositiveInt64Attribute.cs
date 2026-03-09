namespace UD.Core.Attributes.DataAnnotations
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using static UD.Core.Helper.GlobalConstants;
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Parameter, AllowMultiple = false)]
    public sealed class UDRangePositiveInt64Attribute : RangeAttribute
    {
        public UDRangePositiveInt64Attribute() : base(1, Int64.MaxValue)
        {
            this.ErrorMessage = ValidationErrorMessageConstants.GreaterThenZero;
        }
    }
}