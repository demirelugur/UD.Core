namespace UD.Core.Attributes.DefaultValue
{
    using System.ComponentModel;

    public sealed class UDDefaultMacAttribute : DefaultValueAttribute
    {
        public UDDefaultMacAttribute() : base(typeof(string), "00:00:00:00:00:00") { }
    }
}