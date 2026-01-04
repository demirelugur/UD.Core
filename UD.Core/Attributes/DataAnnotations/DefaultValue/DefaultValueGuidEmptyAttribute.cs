namespace UD.Core.Attributes.DataAnnotations.DefaultValue
{
    using System;
    using System.ComponentModel;
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Parameter, AllowMultiple = false)]
    public sealed class DefaultValueGuidEmptyAttribute : DefaultValueAttribute
    {
        public DefaultValueGuidEmptyAttribute() : base(typeof(Guid), Guid.Empty.ToString()) { }
    }
}