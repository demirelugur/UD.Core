using System.ComponentModel;
using System.Net;
namespace UD.Core.Attributes.DefaultValues
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Parameter, AllowMultiple = false)]
    public sealed class UDDefaultIPAddressNoneAttribute : DefaultValueAttribute
    {
        public UDDefaultIPAddressNoneAttribute() : base(typeof(string), IPAddress.None.ToString()) { }
    }
}