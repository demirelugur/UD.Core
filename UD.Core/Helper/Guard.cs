namespace UD.Core.Helper
{
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;
    using System;
    using System.Linq.Expressions;
    using System.Net;
    using UD.Core.Extensions;
    using static UD.Core.Helper.GlobalConstants;
    using static UD.Core.Helper.OrtakTools;
    public sealed class Guard
    {
        public static void CheckNull(object value, string argname)
        {
            if (value == null || value == DBNull.Value) { throw new ArgumentNullException(argname); }
        }
        public static void CheckEmpty(string value, string argname)
        {
            if (value.IsNullOrEmpty()) { throw new ArgumentNullException(argname, $"\"{argname}\" argümanı boş (null) veya sadece boşluk olamaz!"); }
        }
        public static void CheckEmpty(Guid guid, string argname)
        {
            if (guid == Guid.Empty) { throw new ArgumentNullException(argname, $"\"{argname}\" argümanı \"{Guid.Empty.ToString()}\" değerini alamaz!"); }
        }
        public static void CheckEmpty<T>(ICollection<T> collection, string argname)
        {
            if (collection.IsNullOrCountZero()) { throw new ArgumentNullException(argname, $"\"{argname}\" argümanı boş (null) olamaz ve en az bir öğe içermelidir!"); }
        }
        public static void CheckJson(string json, JTokenType jtokentype, string argname)
        {
            CheckEmpty(json, argname);
            if (!_try.TryJson<JToken>(json, jtokentype, out _)) { throw new JsonReaderException($"\"{argname}\" argümanı, \"JSON\" biçimine uygun olmalı ve türü \"{typeof(JTokenType).FullName}\" olmalıdır!"); }
        }
        public static void CheckPhoneNumberTR(string phonenumberTR, string argname)
        {
            CheckEmpty(phonenumberTR, phonenumberTR);
            if (!_try.TryPhoneNumberTR(phonenumberTR, out _)) { throw new ArgumentException($"\"{argname}\" argümanının değeri telefon numarası \"(5xx) (xxx-xxxx)\" biçimine uygun olmalıdır!", argname, new Exception($"Gelen değer: \"{phonenumberTR}\"")); }
        }
        public static void CheckTCKN(long tckn, string argname)
        {
            CheckZeroOrNegative(tckn, argname);
            if (!tckn.IsTCKimlikNo()) { throw new ArgumentException($"\"{argname}\" argümanı, T.C. Kimlik Numarası biçimine uygun olmalıdır!", argname, new Exception($"Gelen değer: \"{tckn}\"")); }
        }
        public static void CheckVKN(long vkn, string argname)
        {
            CheckZeroOrNegative(vkn, argname);
            if (!vkn.IsVergiKimlikNo()) { throw new ArgumentException($"\"{argname}\" argümanı, T.C. Vergi Kimlik Numarası biçimine uygun olmalıdır!", argname, new Exception($"Gelen değer: \"{vkn}\"")); }
        }
        public static void CheckISBN(string isbn, string argname)
        {
            CheckEmpty(isbn, argname);
            if (!ISBNHelper.IsValid(isbn)) { throw new ArgumentException($"\"{argname}\" argümanı, {_title.isbn} biçimine uygun olmalıdır!", argname, new Exception($"Gelen değer: \"{isbn}\"")); }
        }
        public static void CheckMail(string argname, string mail)
        {
            CheckEmpty(mail, argname);
            if (!mail.IsMail()) { throw new ArgumentException($"\"{argname}\" argümanı, e-Posta yapısına uygun olmalıdır!", argname, new Exception($"Gelen değer: \"{mail}\"")); }
        }
        public static void CheckMailFromHost(string argname, string mail, string host)
        {
            CheckMail(argname, mail);
            CheckEmpty(host, nameof(host));
            if (!mail.IsMailFromHost(host)) { throw new ArgumentException($"\"{argname}\" argümanı, e-Posta(example@{(host[0] == '@' ? host.Substring(1) : host)}) yapısına uygun olmalıdır!", argname, new Exception($"Gelen değer: \"{mail}\"")); }
        }
        public static void CheckUri(string uri, string argname)
        {
            CheckEmpty(uri, argname);
            if (!uri.IsUri()) { throw new ArgumentException($"\"{argname}\" argümanı, URL biçimine uygun olmalıdır!", argname, new Exception($"Gelen değer: \"{uri}\"")); }
        }
        public static void CheckIPAddress(string ipstring, string argname)
        {
            CheckEmpty(ipstring, argname);
            if (!IPAddress.TryParse(ipstring, out _)) { throw new ArgumentException($"\"{argname}\" argümanı, IP adresi biçiminde olmalıdır!", argname); }
        }
        public static void CheckOutOfLength(string value, int maxlength, string argname)
        {
            var _l = value.ToStringOrEmpty().Length;
            if (_l > maxlength) { throw new ArgumentException($"\"{argname}\" argümanı, karakter uzunluğu \"{maxlength}\" değerinden uzun olamaz!", argname, new Exception($"Gelen değer: \"{_l}\"")); }
        }
        public static void CheckOutOfLength<T>(string value, Expression<Func<T, string>> expression) where T : class
        {
            var _p = expression.GetExpressionName();
            var _m = _get.GetStringOrMaxLength<T>(_p);
            CheckZeroOrNegative(_m, _p);
            CheckOutOfLength(value, _m, _p);
        }
        public static void CheckIncludes<T>(string argname, T value, params T[] values)
        {
            if (value.Includes(values)) { throw new ArgumentOutOfRangeException($"\"{argname}\" argümanı, \"{String.Join(", ", values)}\" değerlerinden biri olmamalıdır!", new Exception($"Gelen değer: \"{value}\"")); }
        }
        public static void CheckNotIncludes<T>(string argname, T value, params T[] values)
        {
            if (!value.Includes(values)) { throw new ArgumentOutOfRangeException($"\"{argname}\" argümanı, \"{String.Join(", ", values)}\" değerlerinden biri olabilir!", new Exception($"Gelen değer: \"{value}\"")); }
        }
        public static void CheckRange<TKey>(TKey value, TKey min, TKey max, string argname) where TKey : struct, IComparable<TKey>
        {
            if (value.CompareTo(min) < 0 || value.CompareTo(max) > 0) { throw new ArgumentOutOfRangeException($"\"{argname}\" argümanı, [{min} - {max}] değerleri arasında olmalıdır!", new Exception($"Gelen değer: \"{value}\"")); }
        }
        public static void CheckZero<TKey>(TKey value, string argname) where TKey : struct, IComparable<TKey>
        {
            if (value.CompareTo(default(TKey)) == 0) { throw new ArgumentException($"\"{argname}\" argümanı, \"0 (sıfır)\" olamaz!", argname); }
        }
        public static void CheckZeroOrNegative<TKey>(TKey value, string argname) where TKey : struct, IComparable<TKey>
        {
            CheckZero(value, argname);
            CheckNegative(value, argname);
        }
        public static void CheckNegative<TKey>(TKey value, string argname) where TKey : struct, IComparable<TKey>
        {
            if (value.CompareTo(default(TKey)) < 0) { throw new ArgumentOutOfRangeException($"\"{argname}\" argümanı, negatif olamaz!", new Exception($"Gelen değer: \"{value}\"")); }
        }
        public static void CheckEnumDefined(Type type, object value, string argname)
        {
            CheckNull(type, nameof(type));
            if (!type.IsEnum) { throw new ArgumentException($"\"{type.FullName}\" türü geçerli bir \"{nameof(Enum)}\" türü olmalıdır!", argname); }
            CheckNull(value, argname);
            if (!Enum.IsDefined(type, value)) { throw new ArgumentException($"\"{type.FullName}\" için sağlanan \"{argname}\" argümanının değeri geçersizdir!", argname, new Exception($"Gelen değer: \"{value}\"")); }
        }
        public static void CheckEnumDefined<TEnum>(object value, string argname) where TEnum : Enum => CheckEnumDefined(typeof(TEnum), value, argname);
        public static void CheckEqualCount<T>(ICollection<T> collection1, ICollection<T> collection2)
        {
            CheckEmpty(collection1, nameof(collection1));
            CheckEmpty(collection2, nameof(collection2));
            if (collection1.Count != collection2.Count) { throw new ArgumentException($"\"{nameof(collection1)} ({collection1.Count})\" ve \"{nameof(collection2)} ({collection2.Count})\" nesne sayıları eşit olmalıdır!"); }
        }
        public static void UnSupportLanguage(string value, string argname)
        {
            CheckEmpty(value, argname);
            var _defaultlanguages = new string[] { "tr", "en" };
            if (!_defaultlanguages.Contains(value)) { throw new NotSupportedException($"{argname}; {String.Join(", ", _defaultlanguages)} değerlerinden biri olabilir!", new Exception("Yönetici ile iletişime geçiniz!")); }
        }
    }
}