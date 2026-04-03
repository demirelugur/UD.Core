namespace UD.Core.Helper.Database
{
    public sealed class ChangePropertyInfo
    {
        public object originalValue { get; set; }
        public object currentValue { get; set; }
        public bool isPrimaryKey { get; set; }
        public bool isForeignKey { get; set; }
        public ChangePropertyInfo() : this(default, default, default, default) { }
        public ChangePropertyInfo(object originalValue, object currentValue, bool isPrimaryKey, bool isForeignKey)
        {
            this.originalValue = checkHTML(originalValue);
            this.currentValue = checkHTML(currentValue);
            this.isPrimaryKey = isPrimaryKey;
            this.isForeignKey = isForeignKey;
        }
        private object checkHTML(object value)
        {
            if (value is String _s && ValidationChecks.IsHtml(_s)) { return null; }
            return value;
        }
    }
}