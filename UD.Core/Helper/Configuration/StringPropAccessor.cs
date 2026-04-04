namespace UD.Core.Helper.Configuration
{
    using System.Linq.Expressions;
    using UD.Core.Extensions;
    internal class StringPropAccessor
    {
        public Func<object, string?> Getter { get; set; }
        public Action<object, string?> Setter { get; set; }
        public bool SkipSanitize { get; set; }
        public int MaxLength { get; set; }
        public StringPropAccessor() : this(default, default, default, default) { }
        public StringPropAccessor(Func<object, string?> getter, Action<object, string?> setter, bool skipSanitize, int maxLength)
        {
            this.Getter = getter;
            this.Setter = setter;
            this.SkipSanitize = skipSanitize;
            this.MaxLength = maxLength;
        }
        public static StringPropAccessor[] BuildAccessors(Type type) => type.GetProperties()
        .Where(p => p.PropertyType == typeof(string) && p.IsMapped())
        .Select(p =>
        {
            var instance = Expression.Parameter(typeof(object), "i");
            var casted = Expression.Convert(instance, type);
            var property = Expression.Property(casted, p);
            var getter = Expression.Lambda<Func<object, string?>>(property, instance).Compile();
            var valueParam = Expression.Parameter(typeof(string), "v");
            var assign = Expression.Assign(property, valueParam);
            var setter = Expression.Lambda<Action<object, string?>>(assign, instance, valueParam).Compile();
            return new StringPropAccessor(getter, setter, p.IsSkipSanitize(), p.GetStringOrMaxLength());
        }).ToArray();
    }
}