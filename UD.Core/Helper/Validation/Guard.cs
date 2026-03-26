namespace UD.Core.Helper.Validation
{
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;
    using System;
    using System.Linq.Expressions;
    using System.Net;
    using UD.Core.Extensions;
    using UD.Core.Helper;
    using UD.Core.Helper.Configuration;
    using static UD.Core.Helper.GlobalConstants;
    public sealed class Guard
    {
        public static void ThrowIfNull(object value, string argName)
        {
            if (value == null || value == DBNull.Value) { throw new ArgumentNullException(argName); }
        }
        public static void ThrowIfEmpty(string value, string argName)
        {
            if (value.IsNullOrEmpty())
            {
                if (Guards.IsEnglishDefaultThreadCurrentUICulture) { throw new ArgumentNullException(argName, $"The argument \"{argName}\" cannot be null or just a space!"); }
                throw new ArgumentNullException(argName, $"\"{argName}\" argümanı boş (null) veya sadece boşluk olamaz!");
            }
        }
        public static void ThrowIfEmpty(Guid guid, string argName)
        {
            if (guid == Guid.Empty)
            {
                if (Guards.IsEnglishDefaultThreadCurrentUICulture) { throw new ArgumentNullException(argName, $"The argument \"{argName}\" cannot be \"{Guid.Empty}\"!"); }
                throw new ArgumentNullException(argName, $"\"{argName}\" argümanı \"{Guid.Empty}\" değerini alamaz!");
            }
        }
        public static void ThrowIfEmpty<T>(ICollection<T> collection, string argName)
        {
            if (collection.IsNullOrCountZero())
            {
                if (Guards.IsEnglishDefaultThreadCurrentUICulture) { throw new ArgumentNullException(argName, $"The argument \"{argName}\" cannot be null and must contain at least one item!"); }
                throw new ArgumentNullException(argName, $"\"{argName}\" argümanı boş (null) olamaz ve en az bir öğe içermelidir!");
            }
        }
        public static void ThrowIfNotValidJson(string json, JTokenType jTokenType, string argName)
        {
            if (!Validators.TryJson<JToken>(json, jTokenType, out _))
            {
                if (Guards.IsEnglishDefaultThreadCurrentUICulture) { throw new JsonReaderException($"The argument \"{argName}\" must be in \"JSON\" format and of type \"{typeof(JTokenType).FullName}\"!"); }
                throw new JsonReaderException($"\"{argName}\" argümanı, \"JSON\" biçimine uygun olmalı ve türü \"{typeof(JTokenType).FullName}\" olmalıdır!");
            }
        }
        public static void ThrowIfNotValidPhoneNumberTR(string phoneNumberTR, string argName)
        {
            if (!Validators.TryPhoneNumberTR(phoneNumberTR, out _))
            {
                if (Guards.IsEnglishDefaultThreadCurrentUICulture) { throw new ArgumentException($"The argument \"{argName}\" must be a valid phone number in the format of (xxx) xxx-xxxx!", argName); }
                throw new ArgumentException($"\"{argName}\" argümanının değeri telefon numarası \"(5xx) (xxx-xxxx)\" biçimine uygun olmalıdır!", argName);
            }
        }
        public static void ThrowIfNotValidTCKN(long tckn, string argName)
        {
            if (!tckn.IsTCKimlikNo())
            {
                if (Guards.IsEnglishDefaultThreadCurrentUICulture) { throw new ArgumentException($"The argument \"{argName}\" must be in the format of Turkish Republic Identification Number!", argName); }
                throw new ArgumentException($"\"{argName}\" argümanı, T.C. Kimlik Numarası biçimine uygun olmalıdır!", argName);
            }
        }
        public static void ThrowIfNotValidVKN(long vkn, string argName)
        {
            if (!vkn.IsVergiKimlikNo())
            {
                if (Guards.IsEnglishDefaultThreadCurrentUICulture) { throw new ArgumentException($"The argument \"{argName}\" must be in the format of Turkish Republic Tax Identity Number!", argName); }
                throw new ArgumentException($"\"{argName}\" argümanı, T.C. Vergi Kimlik Numarası biçimine uygun olmalıdır!", argName);
            }
        }
        public static void ThrowIfNotValidISBN(string isbn, string argName)
        {
            if (!ISBNHelper.IsValid(isbn))
            {
                if (Guards.IsEnglishDefaultThreadCurrentUICulture) { throw new ArgumentException($"The argument \"{argName}\" must be in the format of {TitleConstants.Isbn}!", argName); }
                throw new ArgumentException($"\"{argName}\" argümanı, {TitleConstants.Isbn} biçimine uygun olmalıdır!", argName);
            }
        }
        public static void ThrowIfNotValidMAC(string mac, string argName)
        {
            if (!Validators.TryMACAddress(mac, out _))
            {
                if (Guards.IsEnglishDefaultThreadCurrentUICulture) { throw new ArgumentException($"The argument \"{argName}\" must be in the format of a valid MAC address!", argName); }
                throw new ArgumentException($"\"{argName}\" argümanı, geçerli bir {TitleConstants.Mac} adresi biçimine uygun olmalıdır!", argName);
            }
        }
        public static void ThrowIfNotValidMail(string mail, string argName)
        {
            if (!mail.IsMail())
            {
                if (Guards.IsEnglishDefaultThreadCurrentUICulture) { throw new ArgumentException($"The argument \"{argName}\" must be in the format of an e-Mail address!", argName); }
                throw new ArgumentException($"\"{argName}\" argümanı, e-Posta yapısına uygun olmalıdır!", argName);
            }
        }
        public static void ThrowIfNotValidUri(string uriString, string argName)
        {
            if (!uriString.IsUri())
            {
                if (Guards.IsEnglishDefaultThreadCurrentUICulture) { throw new ArgumentException($"The argument \"{argName}\" must be in a valid URL format!", argName); }
                throw new ArgumentException($"\"{argName}\" argümanı, URL biçimine uygun olmalıdır!", argName);
            }
        }
        public static void ThrowIfNotValidIPAddress(string ipString, string argName)
        {
            if (!IPAddress.TryParse(ipString, out _))
            {
                if (Guards.IsEnglishDefaultThreadCurrentUICulture) { throw new ArgumentException($"The argument \"{argName}\" must be in a valid IP address format!", argName); }
                throw new ArgumentException($"\"{argName}\" argümanı, IP adresi biçiminde olmalıdır!", argName);
            }
        }
        public static void ThrowIfNotValidOutOfLength(string value, int maxLength, string argName)
        {
            var l = value.ToStringOrEmpty().Length;
            if (l > maxLength)
            {
                if (Guards.IsEnglishDefaultThreadCurrentUICulture) { throw new ArgumentException($"The argument \"{argName}\" cannot be longer than \"{maxLength}\" characters!", argName); }
                throw new ArgumentException($"\"{argName}\" argümanı, karakter uzunluğu \"{maxLength}\" değerinden uzun olamaz!", argName);
            }
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
            if (value.Includes(values))
            {
                if (Guards.IsEnglishDefaultThreadCurrentUICulture) { throw new ArgumentOutOfRangeException(argName, $"The argument \"{argName}\" cannot be one of the following values: \"{String.Join(", ", values)}\"!"); }
                throw new ArgumentOutOfRangeException(argName, $"\"{argName}\" argümanı, \"{String.Join(", ", values)}\" değerlerinden biri olmamalıdır!");
            }
        }
        public static void ThrowIfNotValidIncludes<T>(string argName, T value, params T[] values)
        {
            if (!value.Includes(values))
            {
                if (Guards.IsEnglishDefaultThreadCurrentUICulture) { throw new ArgumentOutOfRangeException(argName, $"The argument \"{argName}\" can only be one of the following values: \"{String.Join(", ", values)}\"!"); }
                throw new ArgumentOutOfRangeException(argName, $"\"{argName}\" argümanı, \"{String.Join(", ", values)}\" değerlerinden biri olabilir!");
            }
        }
        public static void ThrowIfNotValidRange<TKey>(TKey value, TKey min, TKey max, string argName) where TKey : struct, IComparable<TKey>
        {
            if (value.CompareTo(min) < 0 || value.CompareTo(max) > 0)
            {
                if (Guards.IsEnglishDefaultThreadCurrentUICulture) { throw new ArgumentOutOfRangeException(argName, $"The argument \"{argName}\" must be between the values of [{min} - {max}]!"); }
                throw new ArgumentOutOfRangeException(argName, $"\"{argName}\" argümanı, [{min} - {max}] değerleri arasında olmalıdır!");
            }
        }
        public static void ThrowIfZero<TKey>(TKey value, string argName) where TKey : struct, IComparable<TKey>
        {
            if (value.CompareTo(default) == 0)
            {
                if (Guards.IsEnglishDefaultThreadCurrentUICulture) { throw new ArgumentException($"The argument \"{argName}\" cannot be \"0 (zero)\"!", argName); }
                throw new ArgumentException($"\"{argName}\" argümanı, \"0 (sıfır)\" olamaz!", argName);
            }
        }
        public static void ThrowIfNegative<TKey>(TKey value, string argName) where TKey : struct, IComparable<TKey>
        {
            if (value.CompareTo(default) < 0)
            {
                if (Guards.IsEnglishDefaultThreadCurrentUICulture) { throw new ArgumentOutOfRangeException(argName, $"The argument \"{argName}\" cannot be negative!"); }
                throw new ArgumentOutOfRangeException(argName, $"\"{argName}\" argümanı, negatif olamaz!");
            }
        }
        public static void ThrowIfZeroOrNegative<TKey>(TKey value, string argName) where TKey : struct, IComparable<TKey>
        {
            ThrowIfZero(value, argName);
            ThrowIfNegative(value, argName);
        }
        public static void ThrowIfNotValidEnumDefined(Type enumType, object value, string argName)
        {
            ThrowIfNull(enumType, nameof(enumType));
            if (!enumType.IsEnum)
            {
                if (Guards.IsEnglishDefaultThreadCurrentUICulture) { throw new ArgumentException($"The argument \"{nameof(enumType)}\" must be a valid \"{nameof(Enum)}\" type!", nameof(enumType)); }
                throw new ArgumentException($"\"{enumType.FullName}\" türü geçerli bir \"{nameof(Enum)}\" türü olmalıdır!", nameof(enumType));
            }
            ThrowIfNull(value, argName);
            if (!Enum.IsDefined(enumType, value))
            {
                if (Guards.IsEnglishDefaultThreadCurrentUICulture) { throw new ArgumentException($"The argument \"{argName}\" provided for \"{enumType.FullName}\" is invalid!", argName); }
                throw new ArgumentException($"\"{enumType.FullName}\" için sağlanan \"{argName}\" argümanının değeri geçersizdir!", argName);
            }
        }
        public static void ThrowIfNotValidEnumDefined<TEnum>(object value, string argName) where TEnum : Enum => ThrowIfNotValidEnumDefined(typeof(TEnum), value, argName);
    }
}