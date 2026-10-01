using System;
using System.ComponentModel;
namespace UD.Core.Attributes.DefaultValues
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Parameter, AllowMultiple = false)]
    public sealed class UDDefaultGuidEmptyAttribute : DefaultValueAttribute
    {
        public UDDefaultGuidEmptyAttribute() : base(typeof(Guid), Guid.Empty.ToString()) { }
    }
}