namespace UD.Core.Attributes.DefaultValue
{
    using System;
    using System.ComponentModel;
    using System.Net;

    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Parameter, AllowMultiple = false)]
    public sealed class UDIPAddressAnyAttribute : DefaultValueAttribute
    {
        public UDIPAddressAnyAttribute() : base(typeof(string), IPAddress.Any.ToString()) { }
    }
}