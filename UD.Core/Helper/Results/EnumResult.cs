namespace UD.Core.Helper.Results
{
    using Microsoft.AspNetCore.Http;
    using System;
    using UD.Core.Extensions;
    public class EnumResult : IEquatable<EnumResult>
    {
        #region Equals
        public override bool Equals(object other) => this.Equals(other as EnumResult);
        public override int GetHashCode() => HashCode.Combine(this.Value, this.Text);
        public bool Equals(EnumResult other) => (other != null && this.ToString() == other.ToString());
        #endregion
        public long Value { get; }
        public string Text { get; }
        public string DisplayName { get; }
        public EnumResult() : this(default, "", "") { }
        public EnumResult(long value, string text, string displayName)
        {
            this.Value = value;
            this.Text = text;
            this.DisplayName = displayName;
        }
        public override string ToString() => String.Join("-", this.Value, this.Text);
        /// <summary><paramref name="value"/> için tanımlanan nesneler: EnumResult, IFormCollection, Enum, AnonymousObjectClass</summary>
        public static EnumResult ToEntityFromObject(object value)
        {
            if (value == null) { return new(); }
            if (value is EnumResult _er) { return _er; }
            if (value is IFormCollection _form)
            {
                var (hasError, model, errors) = _form.TryBindFromFormAsync<EnumResult>().GetAwaiter().GetResult();
                if (hasError) { throw errors.ToNestedException(); }
                return model;
            }
            var valueType = value.GetType();
            if (valueType.IsEnum)
            {
                var text = Enum.GetName(valueType, value);
                if (text.IsNullOrEmpty()) { return new(); }
                return new(Convert.ToInt64(value), text, valueType.GetField(text).GetDisplayName());
            }
            return value.ToEnumerable().Select(x => x.ToDynamic()).Select(x => new EnumResult((long)x.Value, (string)x.Text, (string)x.DisplayName)).FirstOrDefault();
        }
    }
}