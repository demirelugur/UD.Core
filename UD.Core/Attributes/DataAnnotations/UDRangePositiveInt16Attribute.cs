namespace UD.Core.Attributes.DataAnnotations
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using UD.Core.Helper;
    using static UD.Core.Helper.GlobalConstants;
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Parameter, AllowMultiple = false)]
    public sealed class UDRangePositiveInt16Attribute : RangeAttribute
    {
        public UDRangePositiveInt16Attribute() : base(1, Int16.MaxValue)
        {
            this.ErrorMessage = (Checks.IsEnglishCurrentUICulture ? "{0} must be a value greater than zero!" : ValidationErrorMessageConstants.GreaterThenZero);
        }
    }
}