namespace UD.Core.Helper.Results
{
    using Microsoft.AspNetCore.Http;
    using System;
    using UD.Core.Extensions;
    public class EnumResult : IEquatable<EnumResult>
    {
        #region Equals
        public override bool Equals(object other) => this.Equals(other as EnumResult);
        public override int GetHashCode() => HashCode.Combine(this.value, this.text);
        public bool Equals(EnumResult other) => (other != null && this.ToString() == other.ToString());
        #endregion
        public long value { get; }
        public string text { get; }
        public string description { get; }
        public EnumResult() : this(default, "", "") { }
        public EnumResult(long value, string text, string description)
        {
            this.value = value;
            this.text = text;
            this.description = description;
        }
        public override string ToString() => String.Join("-", this.value, this.text);
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
                try
                {
                    var text = Enum.GetName(valueType, value);
                    return new(Convert.ToInt64(value), text, valueType.GetField(text).GetDescription());
                }
                catch { return new(); }
            }
            return value.ToEnumerable().Select(x => x.ToDynamic()).Select(x => new EnumResult((long)x.value, (string)x.text, (string)x.description)).FirstOrDefault();
        }
    }
}