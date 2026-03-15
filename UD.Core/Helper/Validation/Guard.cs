namespace UD.Core.Helper.Validation
{
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;
    using System;
    using System.Linq.Expressions;
    using System.Net;
    using UD.Core.Extensions;
    using UD.Core.Helper.Configuration;
    using static UD.Core.Helper.GlobalConstants;
    using static UD.Core.Helper.OrtakTools;
    public sealed class Guard
    {
        public static void ThrowIfNull(object value, string argName)
        {
            if (value == null || value == DBNull.Value) { throw new ArgumentNullException(argName); }
        }
        public static void ThrowIfEmpty(string value, string argName)
        {
            if (value.IsNullOrEmpty()) { throw new ArgumentNullException(argName, $"\"{argName}\" argümanı boş (null) veya sadece boşluk olamaz!"); }
        }
        public static void ThrowIfEmpty(Guid guid, string argName)
        {
            if (guid == Guid.Empty) { throw new ArgumentNullException(argName, $"\"{argName}\" argümanı \"{Guid.Empty}\" değerini alamaz!"); }
        }
        public static void ThrowIfEmpty<T>(ICollection<T> collection, string argName)
        {
            if (collection.IsNullOrCountZero()) { throw new ArgumentNullException(argName, $"\"{argName}\" argümanı boş (null) olamaz ve en az bir öğe içermelidir!"); }
        }
        public static void ThrowIfNotValidJson(string json, JTokenType jTokenType, string argName)
        {
            if (!Validators.TryJson<JToken>(json, jTokenType, out _)) { throw new JsonReaderException($"\"{argName}\" argümanı, \"JSON\" biçimine uygun olmalı ve türü \"{typeof(JTokenType).FullName}\" olmalıdır!"); }
        }
        public static void ThrowIfNotValidPhoneNumberTR(string phoneNumberTR, string argName)
        {
            if (!Validators.TryPhoneNumberTR(phoneNumberTR, out _)) { throw new ArgumentException($"\"{argName}\" argümanının değeri telefon numarası \"(5xx) (xxx-xxxx)\" biçimine uygun olmalıdır!", argName, new Exception($"Gelen değer: \"{phoneNumberTR}\"")); }
        }
        public static void ThrowIfNotValidTCKN(long tckn, string argName)
        {
            if (!tckn.IsTCKimlikNo()) { throw new ArgumentException($"\"{argName}\" argümanı, T.C. Kimlik Numarası biçimine uygun olmalıdır!", argName, new Exception($"Gelen değer: \"{tckn}\"")); }
        }
        public static void ThrowIfNotValidVKN(long vkn, string argName)
        {
            if (!vkn.IsVergiKimlikNo()) { throw new ArgumentException($"\"{argName}\" argümanı, T.C. Vergi Kimlik Numarası biçimine uygun olmalıdır!", argName, new Exception($"Gelen değer: \"{vkn}\"")); }
        }
        public static void ThrowIfNotValidISBN(string isbn, string argName)
        {
            if (!ISBNHelper.IsValid(isbn)) { throw new ArgumentException($"\"{argName}\" argümanı, {TitleConstants.Isbn} biçimine uygun olmalıdır!", argName, new Exception($"Gelen değer: \"{isbn}\"")); }
        }
        public static void ThrowIfNotValidMail(string mail, string argName)
        {
            if (!mail.IsMail()) { throw new ArgumentException($"\"{argName}\" argümanı, e-Posta yapısına uygun olmalıdır!", argName, new Exception($"Gelen değer: \"{mail}\"")); }
        }
        public static void ThrowIfNotValidUri(string uriString, string argName)
        {
            if (!uriString.IsUri()) { throw new ArgumentException($"\"{argName}\" argümanı, URL biçimine uygun olmalıdır!", argName, new Exception($"Gelen değer: \"{uriString}\"")); }
        }
        public static void ThrowIfNotValidIPAddress(string ipString, string argName)
        {
            if (!IPAddress.TryParse(ipString, out _)) { throw new ArgumentException($"\"{argName}\" argümanı, IP adresi biçiminde olmalıdır!", argName); }
        }
        public static void ThrowIfNotValidOutOfLength(string value, int maxLength, string argName)
        {
            var l = value.ToStringOrEmpty().Length;
            if (l > maxLength) { throw new ArgumentException($"\"{argName}\" argümanı, karakter uzunluğu \"{maxLength}\" değerinden uzun olamaz!", argName, new Exception($"Gelen değer: \"{l}\"")); }
        }
        public static void ThrowIfNotValidOutOfLength<T>(string value, Expression<Func<T, string>> expression) where T : class
        {
            var p = expression.GetExpressionName();
            var m = Accessors.GetStringOrMaxLength<T>(p);
            ThrowIfZeroOrNegative(m, p);
            ThrowIfNotValidOutOfLength(value, m, p);
        }
        public static void ThrowIfValidIncludes<T>(string argName, T value, params T[] values)
        {
            if (value.Includes(values)) { throw new ArgumentOutOfRangeException($"\"{argName}\" argümanı, \"{String.Join(", ", values)}\" değerlerinden biri olmamalıdır!", new Exception($"Gelen değer: \"{value}\"")); }
        }
        public static void ThrowIfNotValidIncludes<T>(string argName, T value, params T[] values)
        {
            if (!value.Includes(values)) { throw new ArgumentOutOfRangeException($"\"{argName}\" argümanı, \"{String.Join(", ", values)}\" değerlerinden biri olabilir!", new Exception($"Gelen değer: \"{value}\"")); }
        }
        public static void ThrowIfNotValidRange<TKey>(TKey value, TKey min, TKey max, string argName) where TKey : struct, IComparable<TKey>
        {
            if (value.CompareTo(min) < 0 || value.CompareTo(max) > 0) { throw new ArgumentOutOfRangeException($"\"{argName}\" argümanı, [{min} - {max}] değerleri arasında olmalıdır!", new Exception($"Gelen değer: \"{value}\"")); }
        }
        public static void ThrowIfZero<TKey>(TKey value, string argName) where TKey : struct, IComparable<TKey>
        {
            if (value.CompareTo(default) == 0) { throw new ArgumentException($"\"{argName}\" argümanı, \"0 (sıfır)\" olamaz!", argName); }
        }
        public static void ThrowIfNegative<TKey>(TKey value, string argName) where TKey : struct, IComparable<TKey>
        {
            if (value.CompareTo(default) < 0) { throw new ArgumentOutOfRangeException($"\"{argName}\" argümanı, negatif olamaz!", new Exception($"Gelen değer: \"{value}\"")); }
        }
        public static void ThrowIfZeroOrNegative<TKey>(TKey value, string argName) where TKey : struct, IComparable<TKey>
        {
            ThrowIfZero(value, argName);
            ThrowIfNegative(value, argName);
        }
        public static void ThrowIfNotValidEnumDefined(Type type, object value, string argName)
        {
            ThrowIfNull(type, nameof(type));
            if (!type.IsEnum) { throw new ArgumentException($"\"{type.FullName}\" türü geçerli bir \"{nameof(Enum)}\" türü olmalıdır!", argName); }
            ThrowIfNull(value, argName);
            if (!Enum.IsDefined(type, value)) { throw new ArgumentException($"\"{type.FullName}\" için sağlanan \"{argName}\" argümanının değeri geçersizdir!", argName, new Exception($"Gelen değer: \"{value}\"")); }
        }
        public static void ThrowIfNotValidCheckEnumDefined<TEnum>(object value, string argName) where TEnum : Enum => ThrowIfNotValidEnumDefined(typeof(TEnum), value, argName);
        public static void ThrowIfNotEqualCount<T>(ICollection<T> collection1, ICollection<T> collection2)
        {
            ThrowIfEmpty(collection1, nameof(collection1));
            ThrowIfEmpty(collection2, nameof(collection2));
            if (collection1.Count != collection2.Count) { throw new ArgumentException($"\"{nameof(collection1)} ({collection1.Count})\" ve \"{nameof(collection2)} ({collection2.Count})\" nesne sayıları eşit olmalıdır!"); }
        }
    }
}