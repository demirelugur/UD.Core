namespace UD.Core.Helper.Results
{
    using Microsoft.AspNetCore.Http;
    using System;
    using UD.Core.Extensions;
    public class EnumResult : IEquatable<EnumResult>
    {
        #region Equals
        public override bool Equals(object other) => this.Equals(other as EnumResult);
        public override int GetHashCode() => HashCode.Combine(this.value, this.text, this.description);
        public bool Equals(EnumResult other) => other != null && this.value == other.value && this.text == other.text && this.description == other.description;
        #endregion
        public long value { get; }
        public string text { get; }
        public string description { get; }
        public string descseo => this.description.ToSeoFriendly();
        public EnumResult() : this(default, "", "") { }
        public EnumResult(long value, string text, string description)
        {
            this.value = value;
            this.text = text;
            this.description = description;
        }
        /// <summary><paramref name="value"/> için tanımlanan nesneler: EnumResult, IFormCollection, Enum, AnonymousObjectClass</summary>
        public static EnumResult ToEntityFromObject(object value)
        {
            if (value == null) { return new(); }
            if (value is EnumResult _er) { return _er; }
            if (value is IFormCollection _form)
            {
                return ToEntityFromObject(new
                {
                    value = _form.ParseOrDefault<long>(nameof(value)),
                    text = _form.ParseOrDefault<string>(nameof(text)) ?? "",
                    description = _form.ParseOrDefault<string>(nameof(description)) ?? ""
                });
            }
            var t = value.GetType();
            if (t.IsEnum)
            {
                try
                {
                    var text = Enum.GetName(t, value);
                    return new(Convert.ToInt64(value), text, t.GetField(text).GetDescription());
                }
                catch { return new(); }
            }
            return value.ToEnumerable().Select(x => x.ToDynamic()).Select(x => new EnumResult((long)x.value, (string)x.text, (string)x.description)).FirstOrDefault();
        }
    }
}