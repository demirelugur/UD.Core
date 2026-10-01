using System.ComponentModel;
using System.Net;
namespace UD.Core.Attributes.DefaultValues
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Parameter, AllowMultiple = false)]
    public sealed class UDDefaultIPAddressAnyAttribute : DefaultValueAttribute
    {
        public UDDefaultIPAddressAnyAttribute() : base(typeof(string), IPAddress.Any.ToString()) { }
    }
}