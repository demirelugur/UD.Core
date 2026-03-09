namespace UD.Core.Attributes.DataAnnotations
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using static UD.Core.Helper.GlobalConstants;
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Parameter, AllowMultiple = false)]
    public sealed class UDRangePositiveInt16Attribute : RangeAttribute
    {
        public UDRangePositiveInt16Attribute() : base(1, Int16.MaxValue)
        {
            this.ErrorMessage = ValidationErrorMessageConstants.GreaterThenZero;
        }
    }
}