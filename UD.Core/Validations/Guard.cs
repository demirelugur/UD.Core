using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Runtime.CompilerServices;
using UD.Core.Extensions;
using UD.Core.Helper;
using static UD.Core.Helper.GlobalConstants;
namespace UD.Core.Validations
{
    public sealed class Guard
    {
        public static void ThrowIfEmpty(string value, [CallerArgumentExpression(nameof(value))] string? argName = null)
        {
            if (value.IsNullOrEmpty()) { throw new ArgumentNullException(argName, $"\"{argName}\" argümanı boş (null) veya sadece boşluk olamaz!"); }
        }
        public static void ThrowIfEmpty(Guid guid, [CallerArgumentExpression(nameof(guid))] string? argName = null)
        {
            if (guid == Guid.Empty) { throw new ArgumentNullException(argName, $"\"{argName}\" argümanı \"{Guid.Empty}\" değerini alamaz!"); }
        }
        public static void ThrowIfEmpty<T>(IEnumerable<T> source, [CallerArgumentExpression(nameof(source))] string? argName = null)
        {
            if (source.IsNullOrEmptyOrAllNull()) { throw new ArgumentNullException(argName, $"\"{argName}\" argümanı boş (null) olamaz ve en az bir öğe içermelidir!"); }
        }
        public static void ThrowIfNegative<TKey>(TKey value, [CallerArgumentExpression(nameof(value))] string? argName = null) where TKey : struct, IComparable<TKey>
        {
            if (value.CompareTo(default) < 0) { throw new ArgumentOutOfRangeException(argName, $"\"{argName}\" argümanı, negatif olamaz!"); }
        }
        public static void ThrowIfNotValidEnumDefined(Type enumType, object value, [CallerArgumentExpression(nameof(value))] string? argName = null)
        {
            ThrowIfNull(enumType, nameof(enumType));
            if (!enumType.IsEnum) { throw new ArgumentException($"\"{enumType.FullName}\" türü geçerli bir \"{nameof(Enum)}\" türü olmalıdır!", nameof(enumType)); }
            ThrowIfNull(value, argName);
            if (!Enum.IsDefined(enumType, value)) { throw new ArgumentException($"\"{enumType.FullName}\" için sağlanan \"{argName}\" argümanının değeri geçersizdir!", argName); }
        }
        public static void ThrowIfNotValidEnumDefined<TEnum>(object value, [CallerArgumentExpression(nameof(value))] string? argName = null) where TEnum : Enum => ThrowIfNotValidEnumDefined(typeof(TEnum), value, argName);
        public static void ThrowIfNotValidIban(string iban, [CallerArgumentExpression(nameof(iban))] string? argName = null)
        {
            if (!Checks.IsIBANValid(iban)) { throw new ArgumentException($"\"{argName}\" argümanı, {TitleConstants.Iban} biçimine uygun olmalıdır!", argName); }
        }
        public static void ThrowIfNotValidIncludes<T>(string argName, T value, params T[] values)
        {
            if (!value.Includes(values)) { throw new ArgumentOutOfRangeException(argName, $"\"{argName}\" argümanı, \"{String.Join(", ", values)}\" değerlerinden biri olabilir!"); }
        }
        public static void ThrowIfNotValidIPAddress(string ipString, [CallerArgumentExpression(nameof(ipString))] string? argName = null)
        {
            if (!IPAddress.TryParse(ipString, out _)) { throw new ArgumentException($"\"{argName}\" argümanı, IP adresi biçiminde olmalıdır!", argName); }
        }
        public static void ThrowIfNotValidISBN(string isbn, [CallerArgumentExpression(nameof(isbn))] string? argName = null)
        {
            if (!ISBNHelper.IsValid(isbn)) { throw new ArgumentException($"\"{argName}\" argümanı, {TitleConstants.Isbn} biçimine uygun olmalıdır!", argName); }
        }
        public static void ThrowIfNotValidJson(string json, JTokenType jTokenType, [CallerArgumentExpression(nameof(json))] string? argName = null)
        {
            if (!TryValidators.TryJson<JToken>(json, jTokenType, out _)) { throw new JsonReaderException($"\"{argName}\" argümanı, \"JSON\" biçimine uygun olmalı ve türü \"{typeof(JTokenType).FullName}\" olmalıdır!"); }
        }
        public static void ThrowIfNotValidMAC(string mac, [CallerArgumentExpression(nameof(mac))] string? argName = null)
        {
            if (!TryValidators.TryMACAddress(mac, out _)) { throw new ArgumentException($"\"{argName}\" argümanı, geçerli bir {TitleConstants.Mac} adresi biçimine uygun olmalıdır!", argName); }
        }
        public static void ThrowIfNotValidMail(string mail, [CallerArgumentExpression(nameof(mail))] string? argName = null)
        {
            if (!mail.IsMail()) { throw new ArgumentException($"\"{argName}\" argümanı, e-Posta yapısına uygun olmalıdır!", argName); }
        }
        public static void ThrowIfNotValidOutOfLength(string value, int maxLength, [CallerArgumentExpression(nameof(value))] string? argName = null)
        {
            var l = value.ToStringOrEmpty().Length;
            if (l > maxLength) { throw new ArgumentException($"\"{argName}\" argümanı, karakter uzunluğu \"{maxLength}\" değerinden uzun olamaz!", argName); }
        }
        public static void ThrowIfNotValidOutOfLength<T>(string value, Expression<Func<T, string>> expression) where T : class
        {
            var p = expression.GetMemberName();
            var m = Utilities.GetStringOrMaxLength<T>(p);
            ThrowIfZeroOrNegative(m, p);
            ThrowIfNotValidOutOfLength(value, m, p);
        }
        public static void ThrowIfNotValidPhoneNumberTR(string phoneNumberTR, [CallerArgumentExpression(nameof(phoneNumberTR))] string? argName = null)
        {
            if (!TryValidators.TryPhoneNumberTR(phoneNumberTR, out _)) { throw new ArgumentException($"\"{argName}\" argümanının değeri telefon numarası \"(5xx) (xxx-xxxx)\" biçimine uygun olmalıdır!", argName); }
        }
        public static void ThrowIfNotValidRange<TKey>(TKey value, TKey min, TKey max, [CallerArgumentExpression(nameof(value))] string? argName = null) where TKey : struct, IComparable<TKey>
        {
            if (!value.Between(min, max)) { throw new ArgumentOutOfRangeException(argName, $"\"{argName}\" argümanı, [{min} - {max}] değerleri arasında olmalıdır!"); }
        }
        public static void ThrowIfNotValidTRIdentityNumber(long trIdentityNumber, [CallerArgumentExpression(nameof(trIdentityNumber))] string? argName = null)
        {
            if (!trIdentityNumber.IsTRIdentityNumber()) { throw new ArgumentException($"\"{argName}\" argümanı, T.C. Kimlik Numarası biçimine uygun olmalıdır!", argName); }
        }
        public static void ThrowIfNotValidTRTaxIdentityNumber(long trTaxIdentityNumber, [CallerArgumentExpression(nameof(trTaxIdentityNumber))] string? argName = null)
        {
            if (!trTaxIdentityNumber.IsTRTaxIdentityNumber()) { throw new ArgumentException($"\"{argName}\" argümanı, T.C. Vergi Kimlik Numarası biçimine uygun olmalıdır!", argName); }
        }
        public static void ThrowIfNotValidUri(string uriString, [CallerArgumentExpression(nameof(uriString))] string? argName = null)
        {
            if (!uriString.IsUri()) { throw new ArgumentException($"\"{argName}\" argümanı, URL biçimine uygun olmalıdır!", argName); }
        }
        public static void ThrowIfNull(object value, [CallerArgumentExpression(nameof(value))] string? argName = null)
        {
            if (value == null || value == DBNull.Value) { throw new ArgumentNullException(argName); }
        }
        public static void ThrowIfValidIncludes<T>(string argName, T value, params T[] values)
        {
            if (value.Includes(values)) { throw new ArgumentOutOfRangeException(argName, $"\"{argName}\" argümanı, \"{String.Join(", ", values)}\" değerlerinden biri olmamalıdır!"); }
        }
        public static void ThrowIfZero<TKey>(TKey value, [CallerArgumentExpression(nameof(value))] string? argName = null) where TKey : struct, IComparable<TKey>
        {
            if (value.CompareTo(default) == 0) { throw new ArgumentException($"\"{argName}\" argümanı, \"0 (sıfır)\" olamaz!", argName); }
        }
        public static void ThrowIfZeroOrNegative<TKey>(TKey value, [CallerArgumentExpression(nameof(value))] string? argName = null) where TKey : struct, IComparable<TKey>
        {
            ThrowIfZero(value, argName);
            ThrowIfNegative(value, argName);
        }
    }
}