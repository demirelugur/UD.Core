namespace UD.Core.Helper.Databases
{
    public sealed class ChangePropertyInfo
    {
        public object OriginalValue { get; set; }
        public object CurrentValue { get; set; }
        public ChangePropertyInfo() : this(default, default) { }
        public ChangePropertyInfo(object originalValue, object currentValue)
        {
            this.OriginalValue = originalValue;
            this.CurrentValue = currentValue;
        }
    }
}