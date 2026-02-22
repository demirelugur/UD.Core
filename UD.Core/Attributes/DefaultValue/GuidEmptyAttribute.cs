namespace UD.Core.Attributes.DefaultValue
{
    using System;
    using System.ComponentModel;
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Parameter, AllowMultiple = false)]
    public sealed class GuidEmptyAttribute : DefaultValueAttribute
    {
        public GuidEmptyAttribute() : base(typeof(Guid), Guid.Empty.ToString()) { }
    }
}