using System.ComponentModel;
namespace UD.Core.Attributes.DefaultValues
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Parameter, AllowMultiple = false)]
    public sealed class UDDefaultMacAddressAttribute : DefaultValueAttribute
    {
        public UDDefaultMacAddressAttribute() : base(typeof(string), "00:00:00:00:00:00") { }
    }
}