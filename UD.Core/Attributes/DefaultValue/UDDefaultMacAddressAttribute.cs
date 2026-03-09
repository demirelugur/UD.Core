namespace UD.Core.Attributes.DefaultValue
{
    using System.ComponentModel;
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Parameter, AllowMultiple = false)]
    public sealed class UDDefaultMacAddressAttribute : DefaultValueAttribute
    {
        public UDDefaultMacAddressAttribute() : base(typeof(string), "00:00:00:00:00:00") { }
    }
}