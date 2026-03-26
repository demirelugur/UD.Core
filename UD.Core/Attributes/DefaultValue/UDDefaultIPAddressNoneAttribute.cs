namespace UD.Core.Attributes.DefaultValue
{
    using System;
    using System.ComponentModel;
    using System.Net;
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Parameter, AllowMultiple = false)]
    public sealed class UDDefaultIPAddressNoneAttribute : DefaultValueAttribute
    {
        public UDDefaultIPAddressNoneAttribute() : base(typeof(string), IPAddress.None.ToString()) { }
    }
}