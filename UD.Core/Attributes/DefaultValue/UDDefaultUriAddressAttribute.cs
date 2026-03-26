namespace UD.Core.Attributes.DefaultValue
{
    using System.ComponentModel;
    using static UD.Core.Helper.GlobalConstants;
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Parameter, AllowMultiple = false)]
    public sealed class UDDefaultUriAddressAttribute : DefaultValueAttribute
    {
        public UDDefaultUriAddressAttribute() : base(typeof(string), OtherConstants.Example) { }
    }
}