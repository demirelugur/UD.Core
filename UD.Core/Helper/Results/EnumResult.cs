namespace UD.Core.Helper.Results
{
    using Microsoft.AspNetCore.Http;
    using System;
    using UD.Core.Extensions;
    public class EnumResult : IEquatable<EnumResult>
    {
        #region Equals
        public override bool Equals(object other) => this.Equals(other as EnumResult);
        public override int GetHashCode() => HashCode.Combine(this.vl, this.tx, this.desc);
        public bool Equals(EnumResult other) => other != null && this.vl == other.vl && this.tx == other.tx && this.desc == other.desc;
        #endregion
        /// <summary>
        /// Enum değerinin sayısal karşılığını alır.
        /// </summary>
        public long vl { get; }
        /// <summary>
        /// Enum değerinin metin karşılığını alır.
        /// </summary>
        public string tx { get; }
        /// <summary>
        /// Enum değerinin açıklamasını alır.
        /// </summary>
        public string desc { get; }
        /// <summary>
        /// <see cref="desc"/> değerinin SEO dostu bir dizeye dönüştürür.
        /// </summary>
        public string descseo => this.desc.ToSeoFriendly();
        public EnumResult() : this(default, "", "") { }
        public EnumResult(long vl, string tx, string desc)
        {
            this.vl = vl;
            this.tx = tx;
            this.desc = desc;
        }
        /// <summary>
        /// value için tanımlanan nesneler: EnumResult, IFormCollection, Enum, AnonymousObjectClass
        /// </summary>
        public static EnumResult ToEntityFromObject(object value)
        {
            if (value == null) { return new(); }
            if (value is EnumResult _er) { return _er; }
            if (value is IFormCollection _form)
            {
                return ToEntityFromObject(new
                {
                    vl = _form.ParseOrDefault<long>(nameof(vl)),
                    tx = _form.ParseOrDefault<string>(nameof(tx)) ?? "",
                    desc = _form.ParseOrDefault<string>(nameof(desc)) ?? ""
                });
            }
            var _t = value.GetType();
            if (_t.IsEnum)
            {
                try
                {
                    var _tx = Enum.GetName(_t, value);
                    return new(Convert.ToInt64(value), _tx, _t.GetField(_tx).GetDescription());
                }
                catch { return new(); }
            }
            return value.ToEnumerable().Select(x => x.ToDynamic()).Select(x => new EnumResult((long)x.vl, (string)x.tx, (string)x.desc)).FirstOrDefault();
        }
    }
}