namespace UD.Core.Attributes.DataAnnotations
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using static UD.Core.Helper.GlobalConstants;
    using static UD.Core.Helper.OrtakTools;
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Parameter, AllowMultiple = false)]
    public sealed class UDRangePositiveInt16Attribute : RangeAttribute
    {
        public UDRangePositiveInt16Attribute() : base(1, Int16.MaxValue)
        {
            this.ErrorMessage = (Guards.IsEnglishDefaultThreadCurrentUICulture ? "{0} must be a value greater than zero!" : ValidationErrorMessageConstants.GreaterThenZero);
        }
    }
}