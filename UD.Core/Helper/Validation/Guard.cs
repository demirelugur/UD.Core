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
        public static void CheckNull(object value, string argName)
        {
            if (value == null || value == DBNull.Value) { throw new ArgumentNullException(argName); }
        }
        public static void CheckEmpty(string value, string argName)
        {
            if (value.IsNullOrEmpty()) { throw new ArgumentNullException(argName, $"\"{argName}\" argümanı boş (null) veya sadece boşluk olamaz!"); }
        }
        public static void CheckEmpty(Guid guid, string argName)
        {
            if (guid == Guid.Empty) { throw new ArgumentNullException(argName, $"\"{argName}\" argümanı \"{Guid.Empty.ToString()}\" değerini alamaz!"); }
        }
        public static void CheckEmptyOrCountZero<T>(ICollection<T> collection, string argName)
        {
            if (collection.IsNullOrCountZero()) { throw new ArgumentNullException(argName, $"\"{argName}\" argümanı boş (null) olamaz ve en az bir öğe içermelidir!"); }
        }
        public static void CheckJson(string json, JTokenType jTokenType, string argName)
        {
            CheckEmpty(json, argName);
            if (!Validators.TryJson<JToken>(json, jTokenType, out _)) { throw new JsonReaderException($"\"{argName}\" argümanı, \"JSON\" biçimine uygun olmalı ve türü \"{typeof(JTokenType).FullName}\" olmalıdır!"); }
        }
        public static void CheckPhoneNumberTR(string phoneNumberTR, string argName)
        {
            CheckEmpty(phoneNumberTR, phoneNumberTR);
            if (!Validators.TryPhoneNumberTR(phoneNumberTR, out _)) { throw new ArgumentException($"\"{argName}\" argümanının değeri telefon numarası \"(5xx) (xxx-xxxx)\" biçimine uygun olmalıdır!", argName, new Exception($"Gelen değer: \"{phoneNumberTR}\"")); }
        }
        public static void CheckTCKN(long tckn, string argName)
        {
            CheckZeroOrNegative(tckn, argName);
            if (!tckn.IsTCKimlikNo()) { throw new ArgumentException($"\"{argName}\" argümanı, T.C. Kimlik Numarası biçimine uygun olmalıdır!", argName, new Exception($"Gelen değer: \"{tckn}\"")); }
        }
        public static void CheckVKN(long vkn, string argName)
        {
            CheckZeroOrNegative(vkn, argName);
            if (!vkn.IsVergiKimlikNo()) { throw new ArgumentException($"\"{argName}\" argümanı, T.C. Vergi Kimlik Numarası biçimine uygun olmalıdır!", argName, new Exception($"Gelen değer: \"{vkn}\"")); }
        }
        public static void CheckISBN(string isbn, string argName)
        {
            CheckEmpty(isbn, argName);
            if (!ISBNHelper.IsValid(isbn)) { throw new ArgumentException($"\"{argName}\" argümanı, {TitleConstants.Isbn} biçimine uygun olmalıdır!", argName, new Exception($"Gelen değer: \"{isbn}\"")); }
        }
        public static void CheckMail(string mail, string argName)
        {
            CheckEmpty(mail, argName);
            if (!mail.IsMail()) { throw new ArgumentException($"\"{argName}\" argümanı, e-Posta yapısına uygun olmalıdır!", argName, new Exception($"Gelen değer: \"{mail}\"")); }
        }
        public static void CheckMailFromHost(string mail, string host, string argName)
        {
            CheckMail(mail, argName);
            CheckEmpty(host, nameof(host));
            if (!mail.IsMailFromHost(host)) { throw new ArgumentException($"\"{argName}\" argümanı, e-Posta(example@{(host[0] == '@' ? host.Substring(1) : host)}) yapısına uygun olmalıdır!", argName, new Exception($"Gelen değer: \"{mail}\"")); }
        }
        public static void CheckUri(string uriString, string argName)
        {
            CheckEmpty(uriString, argName);
            if (!uriString.IsUri()) { throw new ArgumentException($"\"{argName}\" argümanı, URL biçimine uygun olmalıdır!", argName, new Exception($"Gelen değer: \"{uriString}\"")); }
        }
        public static void CheckIPAddress(string ipString, string argName)
        {
            CheckEmpty(ipString, argName);
            if (!IPAddress.TryParse(ipString, out _)) { throw new ArgumentException($"\"{argName}\" argümanı, IP adresi biçiminde olmalıdır!", argName); }
        }
        public static void CheckOutOfLength(string value, int maxLength, string argName)
        {
            var l = value.ToStringOrEmpty().Length;
            if (l > maxLength) { throw new ArgumentException($"\"{argName}\" argümanı, karakter uzunluğu \"{maxLength}\" değerinden uzun olamaz!", argName, new Exception($"Gelen değer: \"{l}\"")); }
        }
        public static void CheckOutOfLength<T>(string value, Expression<Func<T, string>> expression) where T : class
        {
            var p = expression.GetExpressionName();
            var m = Accessors.GetStringOrMaxLength<T>(p);
            CheckZeroOrNegative(m, p);
            CheckOutOfLength(value, m, p);
        }
        public static void CheckIncludes<T>(string argName, T value, params T[] values)
        {
            if (value.Includes(values)) { throw new ArgumentOutOfRangeException($"\"{argName}\" argümanı, \"{String.Join(", ", values)}\" değerlerinden biri olmamalıdır!", new Exception($"Gelen değer: \"{value}\"")); }
        }
        public static void CheckNotIncludes<T>(string argName, T value, params T[] values)
        {
            if (!value.Includes(values)) { throw new ArgumentOutOfRangeException($"\"{argName}\" argümanı, \"{String.Join(", ", values)}\" değerlerinden biri olabilir!", new Exception($"Gelen değer: \"{value}\"")); }
        }
        public static void CheckRange<TKey>(TKey value, TKey min, TKey max, string argName) where TKey : struct, IComparable<TKey>
        {
            if (value.CompareTo(min) < 0 || value.CompareTo(max) > 0) { throw new ArgumentOutOfRangeException($"\"{argName}\" argümanı, [{min} - {max}] değerleri arasında olmalıdır!", new Exception($"Gelen değer: \"{value}\"")); }
        }
        public static void CheckZero<TKey>(TKey value, string argName) where TKey : struct, IComparable<TKey>
        {
            if (value.CompareTo(default(TKey)) == 0) { throw new ArgumentException($"\"{argName}\" argümanı, \"0 (sıfır)\" olamaz!", argName); }
        }
        public static void CheckZeroOrNegative<TKey>(TKey value, string argName) where TKey : struct, IComparable<TKey>
        {
            CheckZero(value, argName);
            CheckNegative(value, argName);
        }
        public static void CheckNegative<TKey>(TKey value, string argName) where TKey : struct, IComparable<TKey>
        {
            if (value.CompareTo(default(TKey)) < 0) { throw new ArgumentOutOfRangeException($"\"{argName}\" argümanı, negatif olamaz!", new Exception($"Gelen değer: \"{value}\"")); }
        }
        public static void CheckEnumDefined(Type type, object value, string argName)
        {
            CheckNull(type, nameof(type));
            if (!type.IsEnum) { throw new ArgumentException($"\"{type.FullName}\" türü geçerli bir \"{nameof(Enum)}\" türü olmalıdır!", argName); }
            CheckNull(value, argName);
            if (!Enum.IsDefined(type, value)) { throw new ArgumentException($"\"{type.FullName}\" için sağlanan \"{argName}\" argümanının değeri geçersizdir!", argName, new Exception($"Gelen değer: \"{value}\"")); }
        }
        public static void CheckEnumDefined<TEnum>(object value, string argName) where TEnum : Enum => CheckEnumDefined(typeof(TEnum), value, argName);
        public static void CheckEqualCount<T>(ICollection<T> collection1, ICollection<T> collection2)
        {
            CheckEmptyOrCountZero(collection1, nameof(collection1));
            CheckEmptyOrCountZero(collection2, nameof(collection2));
            if (collection1.Count != collection2.Count) { throw new ArgumentException($"\"{nameof(collection1)} ({collection1.Count})\" ve \"{nameof(collection2)} ({collection2.Count})\" nesne sayıları eşit olmalıdır!"); }
        }
        public static void UnSupportLanguage(string value, string argName)
        {
            CheckEmpty(value, argName);
            var defaultlanguages = new string[] { "tr", "en" };
            if (!defaultlanguages.Contains(value)) { throw new NotSupportedException($"{argName}; {String.Join(", ", defaultlanguages)} değerlerinden biri olabilir!", new Exception("Yönetici ile iletişime geçiniz!")); }
        }
    }
}