namespace UD.Core.Attributes.DefaultValue
{
    using System.ComponentModel;
    using UD.Core.Helper;
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Parameter, AllowMultiple = false)]
    public sealed class UDDefaultUriAddressAttribute : DefaultValueAttribute
    {
        public UDDefaultUriAddressAttribute() : base(typeof(string), GlobalConstants.Example) { }
    }
}