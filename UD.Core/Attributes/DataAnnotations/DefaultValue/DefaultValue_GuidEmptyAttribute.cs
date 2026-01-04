namespace UD.Core.Attributes.DataAnnotations.DefaultValue
{
    using System;
    using System.ComponentModel;
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Parameter, AllowMultiple = false)]
    public sealed class DefaultValue_GuidEmptyAttribute : DefaultValueAttribute
    {
        public DefaultValue_GuidEmptyAttribute() : base(typeof(Guid), Guid.Empty.ToString()) { }
    }
}