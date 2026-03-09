namespace UD.Core.Attributes.DefaultValue
{
    using System;
    using System.ComponentModel;
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Parameter, AllowMultiple = false)]
    public sealed class UDDefaultGuidEmptyAttribute : DefaultValueAttribute
    {
        public UDDefaultGuidEmptyAttribute() : base(typeof(Guid), Guid.Empty.ToString()) { }
    }
}