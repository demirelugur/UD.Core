namespace UD.Core.Helper.Database
{
    public sealed class ChangePropertyInfo
    {
        public string propertyname { get; set; }
        public object originalvalue { get; set; }
        public object currentvalue { get; set; }
        public bool isprimarykey { get; set; }
        public bool isforeignkey { get; set; }
        public ChangePropertyInfo() : this(default, default, default, default) { }
        public ChangePropertyInfo(object originalvalue, object currentvalue, bool isprimarykey, bool isforeignkey)
        {
            this.originalvalue = checkHTML(originalvalue);
            this.currentvalue = checkHTML(currentvalue);
            this.isprimarykey = isprimarykey;
            this.isforeignkey = isforeignkey;
        }
        private object checkHTML(object value)
        {
            if (value is String _s && ValidationChecks.IsHtml(_s)) { return null; }
            return value;
        }
    }
}