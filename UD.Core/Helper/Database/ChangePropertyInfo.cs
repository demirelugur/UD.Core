namespace UD.Core.Helper.Database
{
    public sealed class ChangePropertyInfo
    {
        public object originalValue { get; set; }
        public object currentValue { get; set; }
        public ChangePropertyInfo() : this(default, default) { }
        public ChangePropertyInfo(object originalValue, object currentValue)
        {
            this.originalValue = ChangeEntry.maskHTML(originalValue);
            this.currentValue = ChangeEntry.maskHTML(currentValue);
        }
    }
}