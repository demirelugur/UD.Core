namespace UD.Core.Helper
{
    using Microsoft.AspNetCore.Http;
    using Newtonsoft.Json.Linq;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Data;
    using System.Drawing;
    using System.Globalization;
    using System.Linq;
    using System.Net.Mail;
    using System.Reflection;
    using System.Text.Json;
    using System.Text.RegularExpressions;
    using System.Web;
    using UD.Core.Extensions;
    using UD.Core.Helper.Validation;
    using static UD.Core.Helper.GlobalConstants;
    public sealed class TryValidators
    {
        /// <summary>Verilen nesnenin doğrulama kurallarına göre geçerliliğini kontrol eder. Eğer nesne geçerli değilse, doğrulama hatalarını içeren bir dizi döner.</summary>
        /// <param name="instance">Doğrulama işlemi yapılacak nesne.</param>
        /// <param name="outvalue">Geçersiz olduğu tespit edilen durumlarda hata mesajlarını içeren dizi.</param>
        /// <returns>Doğrulama işlemi sonucunu belirtir; geçerli ise <see langword="true"/>, geçersiz ise <see langword="false"/> döner.</returns>
        public static bool TryValidateObject(object instance, out string[] outvalue)
        {
            var vrs = new List<ValidationResult>();
            if (instance != null) { Validator.TryValidateObject(instance, new(instance), vrs, true); } // Not: TryValidateObject kontrolünde instance içerisinde her hangi bir problem yoksa sonuç true gelmekte
            outvalue = vrs.Select(x => x.ErrorMessage).ToArray();
            return vrs.Count > 0;
        }
        /// <summary>Verilen JSON dizisini belirli bir JToken türüne (<see cref="JTokenType"/>) dönüştürmeye çalışır. Dönüşüm başarılı olursa, dönüştürülen değeri döner.</summary>
        /// <typeparam name="TJToken">Hedef JToken türü.</typeparam>
        /// <param name="json">Dönüştürülmek istenen JSON dizisi.</param>
        /// <param name="jTokenType">Beklenen JToken türü.</param>
        /// <param name="outvalue">Başarılı dönüşümde dönen JToken değeri.</param>
        /// <returns>JSON dizisinin beklenen türe uygun olup olmadığını belirtir; uygun ise <see langword="true"/>, değilse <see langword="false"/> döner.</returns>
        public static bool TryJson<TJToken>(string json, JTokenType jTokenType, out TJToken outvalue) where TJToken : JToken
        {
            try
            {
                var jt = JToken.Parse(json.ToStringOrEmpty());
                var r = jt.Type == jTokenType;
                outvalue = r ? (TJToken)jt : default;
                return r;
            }
            catch
            {
                outvalue = default;
                return false;
            }
        }
        /// <summary>Verilen e-Posta adresinin geçerli bir MailAddress nesnesine dönüştürülmesini sağlar. Eğer dönüşüm başarılı olursa, MailAddress nesnesini döner.</summary>
        /// <param name="address">Geçerliliği kontrol edilecek e-Posta adresi.</param>
        /// <param name="outvalue">Başarılı dönüşümde dönen MailAddress nesnesi.</param>
        /// <returns>e-Posta adresinin geçerli olup olmadığını belirtir; geçerli ise <see langword="true"/>, değilse <see langword="false"/> döner.</returns>
        public static bool TryMailAddress(string address, out MailAddress outvalue)
        {
            try
            {
                outvalue = new(address.ToStringOrEmpty());
                return true;
            }
            catch
            {
                outvalue = default;
                return false;
            }
        }
        /// <summary>
        /// Verilen nesne üzerinde belirtilen anahtar (property veya sözlük elemanı) aranır.
        /// <para>Eğer nesne bir <see cref="IDictionary{String, Object}"/> ise anahtar sözlükte aranır. Eğer normal ya da anonim bir nesne ise reflection ile public özelliklerde aranır.</para>
        /// </summary>
        /// <typeparam name="TKey">Beklenen değer tipi.</typeparam>
        /// <param name="value">Üzerinde arama yapılacak nesne (sözlük, anonim tip, dinamik nesne vb.).</param>
        /// <param name="key">Erişilmek istenen property ya da sözlük anahtar adı.</param>
        /// <param name="outvalue">Bulunursa değerin <typeparamref name="TKey"/> tipinde sonucu, aksi halde varsayılan değer.</param>
        /// <returns>Anahtar bulunduysa ve değer istenen tipe dönüştürülebiliyorsa <see langword="true"/>, aksi halde <see langword="false"/>.</returns>
        public static bool TryGetProperty<TKey>(object value, string key, out TKey outvalue)
        {
            try
            {
                if (value != null && !key.IsNullOrEmpty())
                {
                    if (value is IDictionary<string, object> _dic && _dic.TryGetValue(key, out object _dictval) && _dictval is TKey _tdic)
                    {
                        outvalue = _tdic;
                        return true;
                    }
                    var pi = value.GetType().GetProperty(key, BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);
                    if (pi != null)
                    {
                        var piValue = pi.GetValue(value);
                        if (piValue is TKey _tpi)
                        {
                            outvalue = _tpi;
                            return true;
                        }
                    }
                }
                outvalue = default;
                return false;
            }
            catch
            {
                outvalue = default;
                return false;
            }
        }
        /// <summary>
        /// Türkiye için verilen telefon numarasının geçerli biçimde olup olmadığını kontrol eder.
        /// <para>Geçerli örnekler:</para>
        /// <list type="bullet">
        ///     <item><description>5051234567</description></item>
        ///     <item><description>05051234567</description></item>
        ///     <item><description>905051234567</description></item>
        ///     <item><description>+905051234567</description></item>
        ///     <item><description>00905051234567</description></item>
        /// </list>
        /// </summary>
        /// <param name="phoneNumberTR">Kontrol edilecek telefon numarası.</param>
        /// <param name="outvalue">Geçerli olduğu tespit edilen telefon numarası.</param>
        /// <returns>Telefon numarasının geçerli biçimde olup olmadığını belirtir; geçerli ise <see langword="true"/>, değilse <see langword="false"/> döner.</returns>
        public static bool TryPhoneNumberTR(string phoneNumberTR, out string outvalue)
        {
            phoneNumberTR = new(phoneNumberTR.ToStringOrEmpty().Where(x => (x == '+' || Char.IsDigit(x))).ToArray());
            if (phoneNumberTR.Length == 14 && phoneNumberTR.StartsWith("0090")) { phoneNumberTR = phoneNumberTR.Substring(4); }
            else if (phoneNumberTR.Length == 13 && phoneNumberTR.StartsWith("+90")) { phoneNumberTR = phoneNumberTR.Substring(3); }
            else if (phoneNumberTR.Length == 12 && phoneNumberTR.StartsWith("90")) { phoneNumberTR = phoneNumberTR.Substring(2); }
            else if (phoneNumberTR.Length == 11 && phoneNumberTR[0] == '0') { phoneNumberTR = phoneNumberTR.Substring(1); }
            var r = phoneNumberTR.Length == 10 && Regex.IsMatch(phoneNumberTR, @"^\d+$");
            outvalue = r ? phoneNumberTR : "";
            return r;
        }
        /// <summary>Verilen türün Nullable (null değeri alabilen) olup olmadığını kontrol eder. Eğer tür nullable ise, outvalue parametresine nullable olmayan tür atanır.</summary>
        /// <param name="type">Kontrol edilecek tür.</param>
        /// <param name="outvalue">Nullable olmayan tür, dönüş değeri olarak atanır.</param>
        /// <returns>Tür nullable ise <see langword="true"/>, aksi takdirde <see langword="false"/> döner.</returns>
        public static bool TryTypeIsNullable(Type type, out Type outvalue)
        {
            Guard.ThrowIfNull(type, nameof(type));
            var t = (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>));
            outvalue = (t ? type.GenericTypeArguments[0] : type);
            return t;
        }
        /// <summary>Verilen adresin geçerli bir URI olup olmadığını kontrol eder. Geçerli bir URI ise, outvalue parametresine URI değeri atanır. URI&#39;nın HTTP veya HTTPS protokolüne sahip olması gerekmektedir.</summary>
        /// <param name="uriString">Doğrulanacak adres.</param>
        /// <param name="outvalue">Geçerli URI değeri, dönüş değeri olarak atanır.</param>
        /// <returns>Geçerli bir URI ise <see langword="true"/>, aksi takdirde <see langword="false"/> döner.</returns>
        public static bool TryUri(string uriString, out Uri outvalue)
        {
            try { outvalue = (Uri.TryCreate(uriString.ToStringOrEmpty(), UriKind.Absolute, out Uri _uri) && _uri.Scheme.Includes(Uri.UriSchemeHttp, Uri.UriSchemeHttps)) ? _uri : default; }
            catch { outvalue = default; }
            return outvalue != null;
        }
        /// <summary>Verilen türün (Type) özelliklerinden birincil anahtar (Primary Key) olarak işaretlenmiş olanları bulur. Türün özelliklerini tarar ve <see cref="KeyAttribute"/> ile işaretlenmiş özellikleri döndürür.</summary>
        /// <param name="type">Kontrol edilecek tür.</param>
        /// <param name="outvalue">Birincil anahtar olarak işaretlenmiş özelliklerin (PropertyInfo) dizisi. Tür null ise boş bir dizi, hata durumunda default döner.</param>
        /// <returns>En az bir birincil anahtar bulunursa <see langword="true"/>, aksi takdirde <see langword="false"/> döner.</returns>
        public static bool TryTableisKeyAttribute(Type type, out PropertyInfo[] outvalue)
        {
            try
            {
                outvalue = (type == null ? [] : type.GetProperties().Where(x => x.IsPK()).ToArray());
                return outvalue.Length > 0;
            }
            catch
            {
                outvalue = default;
                return false;
            }
        }
        /// <summary>Belirtilen öğe üzerinde verilen türde bir özel niteliğin (attribute) var olup olmadığını kontrol eder. Eğer nitelik bulunursa, <paramref name="outvalue"/> parametresine niteliğin değeri atanır.</summary>
        /// <typeparam name="T">Öğenin türü.</typeparam>
        /// <typeparam name="Y">Kontrol edilecek özel niteliğin türü.</typeparam>
        /// <param name="element">Özel niteliği kontrol edilecek öğe.</param>
        /// <param name="outvalue">Geçerli özel niteliğin değeri, dönüş değeri olarak atanır.</param>
        /// <returns>Geçerli bir özel nitelik varsa <see langword="true"/>, aksi takdirde <see langword="false"/> döner.</returns>
        public static bool TryCustomAttribute<T, Y>(T element, out Y outvalue) where T : ICustomAttributeProvider where Y : Attribute
        {
            try
            {
                outvalue = element.GetCustomAttributes(typeof(Y), false).Cast<Y>().FirstOrDefault();
                return outvalue != null;
            }
            catch
            {
                outvalue = default;
                return false;
            }
        }
        /// <summary>Verilen byte dizisinden bir resim (<see cref="Image"/>) oluşturmayı dener. Başarılı olursa, <paramref name="outvalue"/> parametresine oluşturulan resim atanır. Resim nesnesinin kullanımında dikkatli olunmalı ve gerektiğinde dispose edilmelidir.</summary>
        /// <param name="bytes">Resim verilerini içeren byte dizisi.</param>
        /// <param name="outvalue">Oluşturulan resim nesnesi, dönüş değeri olarak atanır.</param>
        /// <returns>Resim başarıyla oluşturulursa <see langword="true"/>, aksi takdirde <see langword="false"/> döner.</returns>
        /// <remarks>Not: Image kullanıldığı yerde Dispose edilmelidir!</remarks>
        public static bool TryImage(byte[] bytes, out Image outvalue)
        {
            try
            {
                using (var ms = new MemoryStream(bytes))
                {
                    outvalue = Image.FromStream(ms);
                    return true;
                }
            }
            catch
            {
                outvalue = default;
                return false;
            }
        }
        /// <summary>Verilen değerin geçerli bir MAC adresi olup olmadığını kontrol eder. Eğer geçerliyse, <paramref name="outvalue"/> parametresine temizlenmiş MAC adresi atanır. MAC adresinin belirli bir biçimde olması gerekmektedir.</summary>
        /// <param name="value">Kontrol edilecek MAC adresi.</param>
        /// <param name="outvalue">Geçerli MAC adresi, dönüş değeri olarak atanır.</param>
        /// <returns>Geçerli bir MAC adresi varsa <see langword="true"/>, aksi takdirde <see langword="false"/> döner.</returns>
        public static bool TryMACAddress(string value, out string outvalue)
        {
            try
            {
                value = value.ToStringOrEmpty().ToUpper();
                if (value.Length == MaximumLengthConstants.Mac && Regex.IsMatch(value, @"^([0-9A-F]{2}[:-]){5}([0-9A-F]{2})$"))
                {
                    outvalue = value.Replace("-", ":");
                    return true;
                }
                outvalue = "";
                return false;
            }
            catch
            {
                outvalue = "";
                return false;
            }
        }
        /// <summary>
        /// Verilen değerin Google Maps koordinatı olup olmadığını kontrol eder.
        /// <list type="bullet">
        /// <item>
        /// Eğer değer bir Google Maps URL&#39;si ise ve <c>q</c> parametresinde geçerli enlem-boylam bilgisi bulunuyorsa <see langword="true"/> döner.
        /// </item>
        /// <item>
        /// Eğer değer doğrudan &quot;enlem,boylam&quot; biçiminde geçerli bir koordinat ise, bu değer Google Maps URL&#39;sine dönüştürülerek <paramref name="outvalue"/> parametresine atanır.
        /// </item>
        /// <item>
        /// Geçerli bir koordinat değilse <see langword="false"/> döner.
        /// </item>
        /// </list>
        /// <para>Örnek Geçerli Değerler:</para>
        /// <list type="bullet">
        /// <item><description>https://maps.google.com/?q=40.250230335582955,40.23025617808133</description></item>
        /// <item><description>40.250230335582955,40.23025617808133</description></item>
        /// </list>
        /// </summary>
        /// <param name="value">Kontrol edilecek URL veya &quot;enlem,boylam&quot; biçimindeki metin.</param>
        /// <param name="outvalue">Geçerli ise Google Maps koordinat URL&#39;si; aksi durumda varsayılan değer.</param>
        /// <returns>Geçerli koordinat ise <see langword="true"/>, değilse <see langword="false"/>.</returns>
        public static bool TryGoogleMapsCoordinate(string value, out Uri outvalue)
        {
            try
            {
                var q = (TryUri(value, out Uri _u) && _u.Host.Contains("maps.google.com")) ? (new QueryString(_u.Query).ParseOrDefault<string>("q") ?? "").Split(',') : value.ToStringOrEmpty().Split(',').Select(x => x.ToStringOrEmpty()).Where(x => x != "").ToArray();
                if (q.Length == 2 && q.All(isGoogleMapsCoordinateCheck))
                {
                    outvalue = new($"https://maps.google.com/?q={String.Join(",", q)}");
                    return true;
                }
                outvalue = default;
                return false;
            }
            catch
            {
                outvalue = default;
                return false;
            }
        }
        private static bool isGoogleMapsCoordinateCheck(string value) => (Decimal.TryParse(value.Trim(), NumberStyles.AllowDecimalPoint | NumberStyles.AllowLeadingSign, CultureInfo.InvariantCulture, out decimal _r) && _r >= Convert.ToDecimal(-180) && _r <= Convert.ToDecimal(180));
        /// <summary>
        /// Verilen bir metni, belirtilen diller arasında asenkron olarak çevirir.
        /// <para>Bu metot, çeviri işlemi için Google Çeviri API&#39;sini kullanarak, verilen &quot;<paramref name="value"/>&quot; parametresindeki metni &quot;<paramref name="from"/>&quot; dilinden &quot;<paramref name="to"/>&quot; diline çevirir. Varsayılan olarak &quot;<paramref name="from"/>&quot; dili Türkçe (tr) olarak ayarlanmıştır. Eğer çeviri işlemi başarılı olursa, metnin çevirisi ve işlem durumu döndürülür. Hata durumunda, boş bir değer ve <see langword="false"/> durumu döner.</para>
        /// </summary>
        public static async Task<(bool hasError, string value, Exception ex)> TryGoogleTranslate(string value, TimeSpan timeout, string to = "en", string from = "tr", CancellationToken cancellationToken = default)
        {
            value = value.ToStringOrEmpty();
            to = to.ToStringOrEmpty();
            from = from.ToStringOrEmpty();
            if (value == "" || to == "" || from == "")
            {
                if (ValidationChecks.IsEnglishCurrentUICulture) { return (true, "", new ArgumentException("Value, to and from parameters cannot be empty.")); }
                return (true, "", new ArgumentException("Value, to ve from parametreleri boş olamaz."));
            }
            try
            {
                using (var client = new HttpClient
                {
                    Timeout = timeout,
                    DefaultRequestHeaders = { UserAgent = { new("Mozilla", "4.0") } }
                })
                {
                    var response = await client.GetStringAsync($"https://translate.googleapis.com/translate_a/single?client=gtx&sl={from}&tl={to}&dt=t&q={Uri.EscapeDataString(HttpUtility.HtmlEncode(value))}", cancellationToken);
                    using (var doc = JsonDocument.Parse(response))
                    {
                        return (false, doc.RootElement[0][0][0].GetString().ToStringOrEmpty(), null);
                    }
                }
            }
            catch (Exception ex) { return (true, "", ex); }
        }
    }
}