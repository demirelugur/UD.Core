namespace UD.Core.Attributes.DataAnnotations
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using static UD.Core.Helper.GlobalConstants;
    using static UD.Core.Helper.OrtakTools;
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Parameter, AllowMultiple = false)]
    public sealed class UDRangePositiveInt32Attribute : RangeAttribute
    {
        public UDRangePositiveInt32Attribute() : base(1, Int32.MaxValue)
        {
            this.ErrorMessage = (Guards.IsEnglishDefaultThreadCurrentUICulture ? "{0} must be a value greater than zero!" : ValidationErrorMessageConstants.GreaterThenZero);
        }
    }
}