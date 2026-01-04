namespace UD.Core.Attributes.DataAnnotations
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using static UD.Core.Helper.GlobalConstants;
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Parameter, AllowMultiple = false)]
    public sealed class Validation_RangePositiveInt32Attribute : RangeAttribute
    {
        public Validation_RangePositiveInt32Attribute() : base(1, Int32.MaxValue)
        {
            this.ErrorMessage = _validationerrormessage.rangepositive;
        }
    }
}