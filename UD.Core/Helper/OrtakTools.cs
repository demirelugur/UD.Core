namespace UD.Core.Helper
{
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.StaticFiles;
    using Microsoft.Data.SqlClient;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Data;
    using System.Drawing;
    using System.Globalization;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Net.Mail;
    using System.Reflection;
    using System.Security.Cryptography;
    using System.Text;
    using System.Text.Json;
    using System.Text.RegularExpressions;
    using System.Transactions;
    using System.Web;
    using UD.Core.Enums;
    using UD.Core.Extensions;
    using static UD.Core.Helper.GlobalConstants;
    public sealed class OrtakTools
    {
        public sealed class _file
        {
            /// <summary>
            /// Verilen fiziksel dosya yolunda bir dosya varsa onu siler.
            /// </summary>
            /// <param name="physicallypath">Silinecek dosyanın fiziksel yolu.</param>
            public static void FileExistsThenDelete(string physicallypath) { if (File.Exists(physicallypath)) { File.Delete(physicallypath); } }
            /// <summary>
            /// Verilen klasör yolunda bir klasör varsa, isteğe bağlı olarak içindekilerle birlikte siler.
            /// </summary>
            /// <param name="physicallypath">Silinecek klasörün fiziksel yolu.</param>
            /// <param name="recursive">Eğer <see langword="true"/> verilirse, dizin ve altındaki tüm dosyalar ve alt dizinler silinir. <see langword="false"/> verilirse, dizin yalnızca boşsa silinir; aksi halde bir <see cref="IOException"/> fırlatılır.</param>
            public static void DirectoryExistsThenDelete(string physicallypath, bool recursive) { if (Directory.Exists(physicallypath)) { Directory.Delete(physicallypath, recursive); } }
            /// <summary>
            /// Verilen fiziksel dosya yolunda klasör mevcut değilse, ilgili klasörü ve varsa üst dizinlerini oluşturur.
            /// </summary>
            /// <param name="physicallypath">Oluşturulacak klasörün fiziksel yolu.</param>
            public static void DirectoryCreate(string physicallypath)
            {
                var _di = new DirectoryInfo(physicallypath);
                if (_di.Parent != null) { DirectoryCreate(_di.Parent.FullName); }
                if (!_di.Exists) { _di.Create(); }
            }
        }
        public sealed class _get
        {
            /// <summary>
            /// Veritabanı bağlantı dizesi oluşturur. 
            /// <para><c>Data Source=###;Initial Catalog=###;Persist Security Info=True;User ID=###;Password=###;MultipleActiveResultSets=True;TrustServerCertificate=True</c></para>
            /// </summary>
            /// <param name="datasource">Veritabanı sunucu adı veya IP adresi (DataSource)</param>
            /// <param name="initialcatalog">Bağlanılacak veritabanı adı (Initial Catalog)</param>
            /// <param name="userid">Veritabanı kullanıcı adı (User ID)</param>
            /// <param name="password">Veritabanı kullanıcı şifresi (Password)</param>
            /// <returns>Oluşturulan SQL bağlantı dizesini döndürür</returns>
            /// <remarks>
            /// Oluşturulan bağlantı dizesinde aşağıdaki özellikler ayarlanır:
            /// <list type="bullet">
            /// <item><description><b>PersistSecurityInfo</b>: Güvenlik bilgilerinin bağlantı dizesinde kalması sağlanır</description></item>
            /// <item><description><b>MultipleActiveResultSets</b>: Aynı bağlantı üzerinde birden fazla aktif sonuç kümesine izin verilir (MARS)</description></item>
            /// <item><description><b>TrustServerCertificate</b>: Sunucu sertifikasının doğrulanmadan güvenilir kabul edilmesi sağlanır</description></item>
            /// </list>
            /// </remarks>
            public static string GetConnectionString(string datasource, string initialcatalog, string userid, string password) => new SqlConnectionStringBuilder
            {
                DataSource = datasource,
                InitialCatalog = initialcatalog,
                UserID = userid,
                Password = password,
                PersistSecurityInfo = true,
                MultipleActiveResultSets = true,
                TrustServerCertificate = true
            }.ToString();
            /// <summary>
            /// Verilen sınıfın belirtilen özelliğindeki maksimum karakter uzunluğunu döner. Eğer <see cref="StringLengthAttribute"/> veya <see cref="MaxLengthAttribute"/> gibi uzunluk sınırlayıcı öznitelikler atanmışsa, bu değeri alır. Aksi takdirde 0 döner.
            /// </summary>
            /// <typeparam name="T">Kontrol edilecek sınıf türü.</typeparam>
            /// <param name="name">Kontrol edilecek özelliğin adı.</param>
            /// <returns>Özellik için maksimum uzunluk değeri; öznitelik bulunmazsa 0 döner.</returns>
            public static int GetStringOrMaxLength<T>(string name) where T : class => typeof(T).GetProperty(name).GetStringOrMaxLength();
            /// <summary>
            /// Verilen sınıfın belirli bir string ifadesi için maksimum karakter uzunluğunu döner. <see cref="StringLengthAttribute"/> veya <see cref="MaxLengthAttribute"/> atanmışsa bu değeri alır, aksi takdirde 0 döner.
            /// </summary>
            /// <typeparam name="T">Kontrol edilecek sınıf türü.</typeparam>
            /// <param name="expression">Özellik ismini içeren ifade.</param>
            /// <returns>Özellik için maksimum uzunluk değeri; öznitelik bulunmazsa 0 döner.</returns>
            public static int GetStringOrMaxLength<T>(Expression<Func<T, string>> expression) where T : class => GetStringOrMaxLength<T>(expression.GetExpressionName());
            /// <summary>
            /// Verilen string dizisinden kısaltma oluşturur. Her bir kelimenin baş harfini alarak noktalarla ayrılmış bir kısaltma döner. Boş veya null değerler atlanır.
            /// <example>Örnek: <br />Uğur DEMİREL -> U.D <br />Mustafa Kemal ATATÜRK -> MK.A</example>
            /// </summary>
            /// <param name="names">Kısaltma için kullanılacak isim dizisi.</param>
            /// <returns>Verilen isimlerin baş harflerinden oluşan kısaltma. Eğer parametreler boş veya geçersizse boş string döner.</returns>
            public static string GetNameInitials(params string[] names)
            {
                names = (names ?? Array.Empty<string>()).Select(x => x.ToStringOrEmpty()).Where(x => x != "").ToArray();
                if (names.Length == 0) { return ""; }
                return String.Join(".", names.Select(x => String.Join("", x.ToUpper().Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).Select(x => x[0]).ToArray())));
            }
            /// <summary>
            /// Belirtilen dil koduna göre CultureInfo nesnesini döner. Dil kodu &quot;tr&quot; veya &quot;en&quot; olabilir. Diğer diller desteklenmemektedir.
            /// </summary>
            /// <param name="dil">Dil kodu (örnek: &quot;tr&quot;, &quot;en&quot;)</param>
            /// <returns>CultureInfo nesnesi</returns>
            /// <exception cref="NotSupportedException">Eğer dil desteklenmiyorsa hata fırlatılır</exception>
            public static CultureInfo GetCultureInfo(string dil)
            {
                Guard.UnSupportLanguage(dil, nameof(dil));
                if (dil == "en") { return new("en-US"); }
                return new("tr-TR");
            }
            /// <summary>
            /// Verilen nesneden &quot;isldate&quot; ve &quot;isluser&quot; bilgilerini çıkarır ve bu bilgileri belirtilen tarih formatında birleştirerek string olarak döndürür. Nesne tipi IFormCollection, JToken, Tuple, ValueTuple veya bir enumerable koleksiyon olabilir. Eğer nesne null ise, &quot;isldate&quot; geçerli bir tarih değilse veya &quot;isluser&quot; boşsa, boş string döndürür.
            /// </summary>
            /// <param name="model">Bilgilerin alınacağı nesne.</param>
            /// <param name="dateformat">Tarih biçimi.</param>
            /// <returns>&quot;isldate&quot; ve &quot;isluser&quot; bilgilerinin birleştirilmiş string hali; geçerli veri yoksa boş string.</returns>
            public static string GetIslemInfoFromObject(object model, string dateformat = _date.ddMMyyyy_HHmmss)
            {
                if (model == null) { return ""; }
                if (model is IFormCollection _form)
                {
                    return GetIslemInfoFromObject(new
                    {
                        isldate = _form.ParseOrDefault<DateTime>("isldate"),
                        isluser = _form.ParseOrDefault<string>("isluser") ?? ""
                    }, dateformat);
                }
                if (model is JToken _j)
                {
                    return GetIslemInfoFromObject(new
                    {
                        isldate = _j["isldate"].ToEnumerable().Select(x => (x != null && x.Type == JTokenType.Date) ? Convert.ToDateTime(x) : DateTime.MinValue).FirstOrDefault(),
                        isluser = _j["isluser"].ToEnumerable().Select(x => (x != null && x.Type == JTokenType.String) ? Convert.ToString(x) : "").FirstOrDefault()
                    }, dateformat);
                }
                var _type = model.GetType();
                if (_type.FullName.StartsWith("System.Tuple") || _type.FullName.StartsWith("System.ValueTuple"))
                {
                    var _args = _type.GetGenericArguments();
                    if (_args.Length >= 2)
                    {
                        if (_args[0] == typeof(DateTime) && _args[1] == typeof(string))
                        {
                            dynamic _t = model;
                            return GetIslemInfoFromObject(new
                            {
                                isldate = (DateTime)_t.Item1,
                                isluser = (string)_t.Item2
                            }, dateformat);
                        }
                        if (_args[0] == typeof(string) && _args[1] == typeof(DateTime))
                        {
                            dynamic _t = model;
                            return GetIslemInfoFromObject(new
                            {
                                isldate = (DateTime)_t.Item2,
                                isluser = (string)_t.Item1
                            }, dateformat);
                        }
                    }
                }
                return model.ToEnumerable().Select(x => x.ToDynamic()).Select(x => new
                {
                    isldate = (DateTime)x.isldate,
                    isluser = (string)x.isluser
                }).Select(x => String.Join(", ", new string[] { (x.isldate.Ticks > 0 ? x.isldate.ToString(dateformat) : ""), x.isluser.ToStringOrEmpty() }.Where(y => y != "").ToArray())).FirstOrDefault() ?? "";
            }
            /// <summary>
            /// Sadece uzantıdan MIME type döner. Eğer herhangi bir kayıt bulunamazsa &quot;application/octet-stream&quot; değerini döner
            /// </summary>
            /// <param name="extension">Dosya uzantısı (.txt, .pdf vb.)</param>
            /// <returns>MIME type değeri</returns>
            public static string GetMimeTypeByExtension(string extension)
            {
                extension = extension.ToStringOrEmpty().ToLower();
                if (extension != "")
                {
                    if (extension[0] != '.') { extension = String.Concat(".", extension); }
                    if (new FileExtensionContentTypeProvider().Mappings.TryGetValue(extension, out string _v)) { return _v; }
                }
                return "application/octet-stream";
            }
            /// <summary>
            /// Kimlik kartı veya nüfus cüzdanı seri numarasını maskeleme işlemi yapar. İsteğe bağlı olarak kimlik türü ve dil bilgisi ile birlikte açıklama ekler.
            /// </summary>
            /// <param name="cuzdanserino">Maske uygulanacak cüzdan seri numarası.</param>
            /// <param name="kimliktipi">Kimlik türü (yeni kimlik kartı veya eski nüfus cüzdanı).</param>
            /// <param name="showfull"><see langword="true"/> ise seri numarasının tamamı döner; <see langword="false"/> ise ilk 3 hane açık, kalan kısım * ile gizlenmiş döner.
            /// </param>
            /// <param name="dil">Kimlik türü açıklamasının dili (&quot;tr&quot; veya &quot;en&quot;).</param>
            /// <returns>Maske uygulanmış cüzdan seri numarası, opsiyonel olarak kimlik türü bilgisiyle birlikte. Geçersizse boş string döner.</returns>
            public static string MaskedCuzdanSeriNo(string cuzdanserino, NVI_KimlikTypes? kimliktipi, bool showfull, string dil)
            {
                var _cs = cuzdanserino.ToStringOrEmpty();
                if (_cs == "") { return ""; }
                if (!showfull) { _cs = String.Concat(_cs.Substring(0, 3), new('*', _cs.Length - 3)); }
                if (!kimliktipi.HasValue) { return _cs; }
                Guard.UnSupportLanguage(dil, nameof(dil));
                string _t;
                switch (kimliktipi.Value)
                {
                    case NVI_KimlikTypes.yeni: _t = (dil == "en" ? "New ID Card" : "Yeni Kimlik Kartı"); break;
                    case NVI_KimlikTypes.eski: _t = (dil == "en" ? "Old Identity Card" : "Eski Nüfus Cüzdanı"); break;
                    default: throw _other.ThrowNotSupportedForEnum<NVI_KimlikTypes>();
                }
                return $"{_cs} ({_t})";
            }
            /// <summary>
            /// Doğum tarihini maskeler veya tam tarih olarak döndürür.
            /// </summary>
            /// <param name="dogumtarih">Maskelenecek veya gösterilecek doğum tarihi.</param>
            /// <param name="showfull">Eğer <see langword="true"/> ise doğum tarihi tam olarak (dd.MM.yyyy) döner. Eğer <see langword="false"/> ise tarih maskelenir: gün olduğu gibi, ay &#39;**&#39; ile gizlenir, yılın ilk iki hanesi gösterilir ve son iki hanesi &#39;**&#39; ile gizlenir</param>
            /// <returns>Maskelenmiş veya tam doğum tarihi stringi.</returns>
            public static string MaskedDogumTarih(DateOnly dogumtarih, bool showfull)
            {
                var _d = dogumtarih.ToString(_date.ddMMyyyy);
                if (showfull) { return _d; }
                return $"{_d.Substring(0, 2)}.**.{_d.Substring(6, 2)}**";
            }
            /// <summary>
            /// Türkiye biçimine uygun telefon numarasını maskeleme işlemi yapar.
            /// </summary>
            /// <param name="phonenumberTR">Maske uygulanacak telefon numarası (ülke kodu dahil).</param>
            /// <param name="showfull"><see langword="true"/> ise numara güzelleştirilmiş (biçimlenmiş) haliyle döner; <see langword="false"/> ise numaranın bazı bölümleri * ile gizlenmiş şekilde döner.
            /// </param>
            /// <returns>Maske uygulanmış veya tam telefon numarası. Geçersiz ise boş string döner.</returns>
            public static string MaskedPhoneNumberTR(string phonenumberTR, bool showfull)
            {
                if (showfull) { return phonenumberTR.BeautifyPhoneNumberTR(); }
                return (_try.TryPhoneNumberTR(phonenumberTR, out string _t) ? $"(**{_t.Substring(2, 1)}) {_t.Substring(3, 1)}**-*{_t.Substring(8, 2)}" : "");
            }
            /// <summary>
            /// Verilen sayısal kimlik numarasını (TCKN veya VKN) maskeler. TCKN olarak doğrulanırsa orta kısım 6 adet &#39;*&#39;, VKN olarak doğrulanırsa 5 adet &#39;*&#39; ile gizlenir. Eğer <paramref name="showfull"/> true ise numara olduğu gibi döndürülür. Geçerli bir TCKN veya VKN değilse boş string döndürülür.
            /// </summary>
            /// <param name="value">Maskelenecek kimlik numarası.</param>
            /// <param name="showfull">true ise maskesiz tam numara döndürülür; false ise ilgili kısım maskelenir.</param>
            /// <returns>Maskelenmiş veya tam kimlik numarası. Geçerli bir TCKN/VKN değilse boş string döner.</returns>
            public static string MaskedTCKNorVKN(long value, bool showfull)
            {
                var count = 0;
                if (value.IsTCKimlikNo()) { count = 6; }
                else if (value.IsVergiKimlikNo()) { count = 5; }
                if (count == 0) { return ""; }
                var _t = value.ToString();
                if (showfull) { return _t; }
                return String.Concat(_t.Substring(0, 3), new('*', count), _t.Substring(9, 2));
            }
            /// <summary>Verilen metindeki Türkçe özel karakterleri (Ç, ç, Ğ, ğ, İ, ı, Ö, ö, Ş, ş, Ü, ü) karşılık gelen İngilizce karakterlerle (C, c, G, g, I, i, O, o, S, s, U, u) değiştirir. Eğer metin null ise, boş bir string döner.</summary>
            /// <param name="value">Türkçe karakterlerin değiştirileceği metin.</param>
            /// <returns>Değiştirilmiş metni döner.</returns>
            public static string ReplaceTurkishChars(string value) => value.ToStringOrEmpty().Replace('Ç', 'C').Replace('ç', 'c').Replace('Ğ', 'G').Replace('ğ', 'g').Replace('İ', 'I').Replace('ı', 'i').Replace('Ö', 'O').Replace('ö', 'o').Replace('Ş', 'S').Replace('ş', 's').Replace('Ü', 'U').Replace('ü', 'u');
        }
        public sealed class _is
        {
            /// <summary>
            /// Verilen string&#39;in HTML tag&#39;leri içerip içermediğini kontrol eder. String null, boş veya yalnızca boşluklardan oluşuyorsa <see langword="false"/> döner. HTML tag&#39;leri, düzenli ifade (regex) kullanılarak tespit edilir.
            /// <code>(!s.IsNullOrEmpty_string() &amp;&amp; Regex.IsMatch(s, @&quot;&lt;/?\w+\s*[^&gt;]*&gt;&quot;, RegexOptions.Compiled))</code>
            /// </summary>
            /// <param name="s">Kontrol edilecek string.</param>
            /// <returns>String HTML tag&#39;i içeriyorsa <see langword="true"/>, aksi takdirde <see langword="false"/> döner.</returns>
            public static bool IsHtml(string s) => (!s.IsNullOrEmpty() && Regex.IsMatch(s, @"</?\w+\s*[^>]*>", RegexOptions.Compiled));
            /// <summary>
            /// Belirtilen dosyanın tarayıcı tarafından indirilebilir olup olmadığını kontrol eder. PDF veya görüntü dosyaları (image/*) indirme işlemi için uygun değilse <see langword="false"/> döner. Aksi takdirde <see langword="true"/> döner.
            /// </summary>
            /// <param name="path">Kontrol edilecek dosyanın yolu.</param>
            /// <returns>Dosya indirilebilir ise <see langword="true"/>, değilse <see langword="false"/>.</returns>
            public static bool IsDownloadableFile(string path)
            {
                if (path.IsNullOrEmpty()) { return false; }
                try
                {
                    var _uzn = Path.GetExtension(path).ToLower();
                    if (_uzn == ".pdf") { return false; }
                    if (new FileExtensionContentTypeProvider().Mappings.Any(x => x.Key == _uzn && x.Value.StartsWith("image/"))) { return false; }
                    return true;
                }
                catch { return false; }
            }
        }
        public sealed class _other
        {
            /// <summary>
            /// Asenkron işlemler için TransactionScope oluşturur. TransactionScope, işlem bütünlüğünü sağlamak için kullanılır. Bu metod, asenkron işlemlerin TransactionScope ile birlikte kullanılabilmesi için ayarlanmıştır.
            /// <code>new TransactionScope(TransactionScopeOption.Required, TransactionScopeAsyncFlowOption.Enabled);</code>
            /// </summary>
            public static TransactionScope TransactionScopeForAsync => new(TransactionScopeOption.Required, TransactionScopeAsyncFlowOption.Enabled);
            /// <summary>
            /// Bir değeri belirtilen türe dönüştürür. Eğer değer null ise ve tip nullable ise null döner. Enum türlerini destekler ve enum değerlerini ilgili türe dönüştürür.
            /// </summary>
            /// <param name="value">Dönüştürülecek değer</param>
            /// <param name="type">Dönüştürülecek hedef tür</param>
            /// <returns>Dönüştürülmüş değer</returns>
            public static object ChangeType(object value, Type type)
            {
                var _t = _try.TryTypeIsNullable(type, out Type _genericbasetype);
                if (_t && value == null) { return null; }
                if (_genericbasetype.IsEnum) { return Enum.ToObject(_genericbasetype, value); }
                return Convert.ChangeType(value, _t ? Nullable.GetUnderlyingType(type) : _genericbasetype);
            }
            /// <summary>
            /// Verilen metni Sezar şifreleme algoritması ile şifreler. Belirtilen anahtar (key) değeri kadar harfler kaydırılarak şifreleme yapılır.
            /// </summary>
            /// <param name="value">Şifrelenecek metin</param>
            /// <param name="key">Harflerin kaydırılacağı değer</param>
            /// <returns>Şifrelenmiş metin</returns>
            public static string CaesarCipherOperation(string value, int key)
            {
                if (key < 0) { return CaesarCipherOperation(value, key + 26); }
                var _r = "";
                foreach (var item in value.ToStringOrEmpty().ToCharArray())
                {
                    if ((item >= 'A' && item <= 'Z')) { _r = String.Concat(_r, Convert.ToChar(((item - 'A' + key) % 26) + 'A').ToString()); }
                    else if ((item >= 'a' && item <= 'z')) { _r = String.Concat(_r, Convert.ToChar(((item - 'a' + key) % 26) + 'a').ToString()); }
                    else { _r = String.Concat(_r, item.ToString()); }
                }
                return _r;
            }
            /// <summary>
            /// Belirtilen nesnenin property adını kullanarak ilgili property&#39;sine değer atar.
            /// </summary>
            /// <param name="value">Değeri atanac nesne.</param>
            /// <param name="propertyname">Değeri atanacak property&#39;sinin adı.</param>
            /// <param name="data">Property&#39;ye atanacak değer.</param>
            /// <param name="dil">Hata mesajlarının döndürüleceği dil.</param>
            /// <exception cref="ArgumentException"><paramref name="value"/> nesnesi sınıf türünde değilse fırlatılır.</exception>
            /// <exception cref="InvalidOperationException">Property yazılabilir değilse fırlatılır.</exception>
            /// <exception cref="ArgumentNullException"><paramref name="value"/> veya <paramref name="propertyname"/> null ise fırlatılır.</exception>
            public static void SetPropertyValue(object value, string propertyname, object data, string dil)
            {
                Guard.CheckNull(value, nameof(value));
                Guard.CheckEmpty(propertyname, nameof(propertyname));
                Guard.UnSupportLanguage(dil, nameof(dil));
                var _type = value.GetType();
                if (!_type.IsCustomClass()) { throw new ArgumentException(dil == "en" ? $"The \"{nameof(value)}\" argument type must be class!" : $"\"{nameof(value)}\" argümanı türü class olmalıdır!", nameof(value)); }
                var _pi = _type.GetProperty(propertyname);
                Guard.CheckNull(_pi, nameof(_pi));
                if (!_pi.CanWrite) { throw new InvalidOperationException(dil == "en" ? $"The \"{nameof(propertyname)}\" property is not writable!" : $"\"{nameof(propertyname)}\" özelliği yazılabilir değil!"); }
                _pi.SetValue(value, data == null ? null : ChangeType(data, _pi.PropertyType));
            }
            /// <summary>
            /// Enum türleri için desteklenmeyen değer hatası oluşturur. Belirtilen Enum türü ve ek detaylarla birlikte bir hata mesajı üretir.
            /// </summary>
            /// <typeparam name="TEnum">Enum türü (generic).</typeparam>
            /// <param name="details">Hata mesajına eklenecek isteğe bağlı ek detaylar.</param>
            /// <returns>Desteklenmeyen Enum değerine ait NotSupportedException nesnesi döner.</returns>
            public static NotSupportedException ThrowNotSupportedForEnum<TEnum>(params string[] details) where TEnum : Enum
            {
                var _r = new HashSet<string> { typeof(TEnum).FullName, $"{nameof(Enum)} değeri uyumsuzdur!" };
                if (!details.IsNullOrCountZero()) { _r.AddRangeOptimized(details); }
                return new(String.Join(" ", _r));
            }
        }
        public sealed class _to
        {
            /// <summary>
            /// Verilen nesneyi JSON formatına dönüştürür. JSON çıktısı None formatında ve bazı özel ayarlarla döner.
            /// </summary>
            /// <param name="value">JSON&#39;a dönüştürülecek nesne.</param>
            /// <returns>Nesnenin JSON string formatındaki temsili.</returns>
            public static string ToJSON(object value) => JsonConvert.SerializeObject(value, Formatting.None, jsonserializersettings);
            /// <summary>
            /// Verilen string ifadeyi tersine çevirir. Bu metot, Türkçe karakterler (ğ, ü, ş, ç, ö, ı, İ vb.) dahil olmak üzere tüm Unicode metin öğelerini dikkate alarak çalışır. Standart char tabanlı ters çevirme yöntemlerinden farklı olarak <see cref="StringInfo"/> sınıfını kullanır ve her bir metin öğesini (text element) ayrı değerlendirir.
            /// </summary>
            /// <param name="value">Tersine çevrilecek string ifade.</param>
            /// <returns>Ters çevrilmiş string ifade.</returns>
            public static string ToReverse(string value)
            {
                value = value.ToStringOrEmpty();
                if (value == "") { return ""; }
                var _si = new StringInfo(value);
                int _i, _length = _si.LengthInTextElements;
                var _elements = new string[_length];
                for (_i = 0; _i < _length; _i++) { _elements[_i] = _si.SubstringByTextElements(_i, 1); }
                Array.Reverse(_elements);
                return String.Concat(_elements);
            }
            /// <summary>
            /// Verilen nesneyi <see cref="SHA256"/> hash string formatına dönüştürür. Eğer değer null ise boş string döner.
            /// </summary>
            /// <param name="value">Hashlenecek nesne.</param>
            /// <returns>Nesnenin <see cref="SHA256"/> hash string temsili.</returns>
            /// <remarks>Not: MSSQL&#39;deki karşılığı SELECT SUBSTRING([sys].[fn_varbintohexstr](HASHBYTES(&#39;SHA2_256&#39;, &#39;Lorem Ipsum&#39;)), 3, 64) AS HashValue</remarks>
            public static string ToHashSHA256FromObject(object value)
            {
                if (value == null) { return ""; } // SELECT SUBSTRING([sys].[fn_varbintohexstr](HASHBYTES('SHA2_256', 'Lorem Ipsum')), 3, 64)
                var _r = new List<string>();
                foreach (var item in SHA256.HashData(Encoding.UTF8.GetBytes(value is String _s ? _s.Trim() : ToJSON(value)))) { _r.Add(item.ToString("x2")); }
                return String.Join("", _r);
            }
            /// <summary>
            /// Belirtilen enum türündeki değerleri ve karşılık gelen long değerlerini içeren bir sözlük oluşturur.
            /// </summary>
            /// <typeparam name="TEnum">Enum türü.</typeparam>
            /// <returns>Enum isimlerini ve long karşılıklarını içeren sözlük.</returns>
            public static Dictionary<string, long> ToDictionaryFromEnum<TEnum>() where TEnum : Enum
            {
                var _t = typeof(TEnum);
                return ((TEnum[])Enum.GetValues(_t)).Select(x => Convert.ToInt64(_other.ChangeType(x, typeof(long)))).ToDictionary(x => Enum.GetName(_t, x));
            }
            /// <summary>
            /// Verilen nesneyi, özellik isimlerini ve değerlerini içeren bir sözlüğe dönüştürür. Yalnızca özel sınıf türlerinde çalışır.
            /// </summary>
            /// <param name="obj">Dönüştürülecek nesne.</param>
            /// <returns>Nesnenin özellik isimlerini ve değerlerini içeren sözlük.</returns>
            public static Dictionary<string, object> ToDictionaryFromObject(object obj)
            {
                if (obj == null) { return new(); }
                if (obj is Dictionary<string, object> _d) { return _d; }
                var _t = obj.GetType();
                if (_t.IsCustomClass()) { return _t.GetProperties().ToDictionary(x => x.Name, x => x.GetValue(obj)); }
                throw new Exception($"{nameof(obj)} türü uygun biçimde değildir!");
            }
            /// <summary>
            /// Verilen nesneyi SQL parametrelerine dönüştürür. Eğer nesne <see cref="SqlParameter"/> türünde ise doğrudan SQL parametreleri olarak döner. Özel sınıf türlerinde çalışır ve özellik isimlerine göre SQL parametrelerini oluşturur.
            /// <para>obj için tanımlanan nesneler: SqlParameter, IEnumerable&lt;SqlParameter&gt;, IDictionary&lt;string, object&gt;, AnonymousObjectClass</para>
            /// </summary>
            /// <param name="obj">Dönüştürülecek nesne.</param>
            /// <returns>Nesneyi temsil eden SQL parametrelerinin dizisi.</returns>
            public static SqlParameter[] ToSqlParameterFromObject(object obj)
            {
                if (obj == null) { return Array.Empty<SqlParameter>(); }
                if (obj is SqlParameter _sp) { return new SqlParameter[] { _sp }; }
                if (obj is IEnumerable<SqlParameter> _sps) { return _sps.ToArray(); }
                return (obj is IDictionary<string, object> _dic ? _dic : ToDictionaryFromObject(obj)).Select(x => new SqlParameter
                {
                    ParameterName = x.Key,
                    Value = x.Value ?? DBNull.Value
                }).ToArray();
            }
            /// <summary>
            /// Verilen nesneyi DateOnly tipine dönüştürür.
            /// <para>obj için tanımlanan nesneler: DateOnly, DateTime, Int64, String(DateOnly, DateTime, Int64 türlerine uygun biçimde olmalıdır)</para>
            /// </summary>
            /// <param name="obj">Dönüştürülecek nesne.</param>
            /// <returns>DateOnly değeri.</returns>
            public static DateOnly ToDateOnlyFromObject(object obj) => ToDateTimeFromObject(obj, default).ToDateOnly();
            /// <summary>
            /// Verilen nesneyi DateTime tipine dönüştürür ve isteğe bağlı bir zaman değeri ekler.
            /// <para>obj için tanımlanan nesneler: DateTime, DateOnly, Int64, String(DateTime, DateOnly, Int64 türlerine uygun biçimde olmalı)</para>
            /// </summary>
            /// <param name="obj">Dönüştürülecek nesne.</param>
            /// <param name="timeonly">Zaman bilgisi (isteğe bağlı). <paramref name="obj"/> değeri türü DateOnly iken girilecek değer anlamlıdır</param>
            /// <returns>DateTime değeri.</returns>
            public static DateTime ToDateTimeFromObject(object obj, TimeOnly? timeonly)
            {
                if (obj is DateTime _dt) { return _dt; }
                if (obj is DateOnly _do) { return _do.ToDateTime(timeonly ?? default); }
                if (obj is (Byte or Int16 or Int32 or Int64)) { return new(obj.ToLong()); }
                if (obj is String _s)
                {
                    if (DateTime.TryParse(_s, out _dt)) { return _dt; }
                    if (DateOnly.TryParse(_s, out _do)) { return _do.ToDateTime(timeonly ?? default); }
                    if (Int64.TryParse(_s, out long _ticks)) { return new(_ticks); }
                }
                return default;
            }
            /// <summary>
            /// Belirtilen dosya yolundan asenkron olarak bir <see cref="IFormFile"/> nesnesi oluşturur. Bu metod, dosya sistemindeki bir dosyayı bellek akışına okuyarak web formları veya API istekleri için uygun hale getirir.
            /// </summary>
            /// <param name="filepath">Dönüştürülecek dosyanın sistemdeki tam yolu.</param>
            /// <param name="cancellationtoken">Asenkron işlemin iptali için kullanılan sinyal.</param>
            /// <param name="name">IFormFile nesnesine verilecek ad. Varsayılan olarak &quot;file&quot; kullanılır.</param>
            /// <param name="headerdictionary">Oluşturulacak IFormFile için özel başlıkları içeren sözlük. (Opsiyonel)</param>
            /// <returns>Oluşturulan IFormFile nesnesini temsil eden bir <see cref="Task{TResult}"/>.</returns>
            /// <exception cref="ArgumentException">FilePath veya Name parametreleri boş veya null ise fırlatılır.</exception>
            /// <exception cref="FileNotFoundException">Belirtilen dosya yolu mevcut değilse fırlatılır.</exception>
            public static async Task<FormFile> ToIFormFileFromPathAsync(string filepath, CancellationToken cancellationtoken = default, string name = "file", HeaderDictionary? headerdictionary = null)
            {
                Guard.CheckEmpty(filepath, nameof(filepath));
                Guard.CheckEmpty(name, nameof(name));
                if (!File.Exists(filepath)) { throw new FileNotFoundException("Belirtilen dosya bulunamadı!", filepath); }
                var _ms = new MemoryStream();
                using (var fs = new FileStream(filepath, FileMode.Open, FileAccess.Read, FileShare.Read, 4096, true))
                {
                    await fs.CopyToAsync(_ms, cancellationtoken);
                }
                _ms.Position = 0;
                var _provider = new FileExtensionContentTypeProvider();
                return new(_ms, 0, _ms.Length, name, Path.GetFileName(filepath))
                {
                    Headers = headerdictionary ?? new HeaderDictionary(),
                    ContentType = (_provider.TryGetContentType(filepath, out string _contenttype) ? _contenttype : "application/octet-stream")
                };
            }
            /// <summary>
            /// SQL Server&#39;ın sistem tür kimliğini <c>([system_type_id])</c> <see cref="SqlDbType"/> enum değerine dönüştürür.
            /// </summary>
            /// <param name="systemtypeid">SQL Server [sys].[types] tablosundaki [system_type_id] değeri.</param>
            /// <returns>Eşleşen <see cref="SqlDbType"/> enum değeri.</returns>
            /// <exception cref="NotSupportedException">Geçersiz veya desteklenmeyen bir sistem tür kimliği verildiğinde fırlatılır.</exception>
            public static SqlDbType ToSqlDbTypeFromSystemTypeID(int systemtypeid)
            {
                switch (systemtypeid)
                {
                    case 34: return SqlDbType.Image;
                    case 35: return SqlDbType.Text;
                    case 36: return SqlDbType.UniqueIdentifier;
                    case 40: return SqlDbType.Date;
                    case 41: return SqlDbType.Time;
                    case 42: return SqlDbType.DateTime2;
                    case 43: return SqlDbType.DateTimeOffset;
                    case 48: return SqlDbType.TinyInt;
                    case 52: return SqlDbType.SmallInt;
                    case 56: return SqlDbType.Int;
                    case 58: return SqlDbType.SmallDateTime;
                    case 59: return SqlDbType.Real;
                    case 60: return SqlDbType.Money;
                    case 61: return SqlDbType.DateTime;
                    case 62: return SqlDbType.Float;
                    case 99: return SqlDbType.NText;
                    case 104: return SqlDbType.Bit;
                    case 106: return SqlDbType.Decimal;
                    case 122: return SqlDbType.SmallMoney;
                    case 127: return SqlDbType.BigInt;
                    case 165: return SqlDbType.VarBinary;
                    case 167: return SqlDbType.VarChar;
                    case 173: return SqlDbType.Binary;
                    case 175: return SqlDbType.Char;
                    case 189: return SqlDbType.Timestamp;
                    case 231: return SqlDbType.NVarChar;
                    case 239: return SqlDbType.NChar;
                    case 241: return SqlDbType.Xml;
                    default: throw new NotSupportedException($"Geçersiz veya desteklenmeyen {nameof(systemtypeid)}: {systemtypeid}");
                }
            }
            /// <summary>Verilen bir data URI string&#39;ini binary veriye ve MIME tipine dönüştürür. <see cref="IOExtensions.ToBase64StringFromBinary(byte[], string)"/> işleminin tersi </summary>
            /// <param name="datauri">Dönüştürülecek data URI string&#39;i. Biçim: &quot;data:[MIME-type];base64,[base64-encoded-data]&quot;</param>
            /// <param name="dil">Hata mesajlarının dilini belirtir.</param>
            /// <returns>Binary veri (byte[]) ve MIME tipini içeren bir tuple döner.</returns>
            /// <exception cref="ArgumentException">Geçersiz data URI biçimi veya eksik MIME tipi/base64 verisi durumunda fırlatılır.</exception>
            /// <exception cref="ArgumentException">Desteklenmeyen dil belirtildiğinde fırlatılır.</exception>
            public static (byte[] bytes, string mimetype) ToBinaryFromBase64String(string datauri, string dil)
            {
                Guard.UnSupportLanguage(dil, nameof(dil));
                datauri = datauri.ToStringOrEmpty();
                if (datauri == "" || !datauri.StartsWith("data:")) { throw new ArgumentException(dil == "en" ? "Invalid data URI format." : "Geçersiz veri URI formatı."); }
                var _parts = datauri.Substring(5).Split(new string[] { ";base64," }, StringSplitOptions.None);
                if (_parts.Length != 2) { throw new ArgumentException(dil == "en" ? "Invalid data URI format: MIME type or base64 data is missing." : "Geçersiz veri URI formatı: MIME tipi veya base64 verisi eksik."); }
                return (Convert.FromBase64String(_parts[1]), _parts[0]);
            }
        }
        public sealed class _try
        {
            /// <summary>
            /// Verilen nesnenin doğrulama kurallarına göre geçerliliğini kontrol eder. Eğer nesne geçerli değilse, doğrulama hatalarını içeren bir dizi döner.
            /// </summary>
            /// <param name="instance">Doğrulama işlemi yapılacak nesne.</param>
            /// <param name="outvalue">Geçersiz olduğu tespit edilen durumlarda hata mesajlarını içeren dizi.</param>
            /// <returns>Doğrulama işlemi sonucunu belirtir; geçerli ise <see langword="true"/>, geçersiz ise <see langword="false"/> döner.</returns>
            public static bool TryValidateObject(object instance, out string[] outvalue)
            {
                var _vrs = new List<ValidationResult>();
                Validator.TryValidateObject(instance, new(instance), _vrs, true); // Not: TryValidateObject kontrolünde instance içerisinde her hangi bir problem yoksa sonuç true gelmekte
                outvalue = _vrs.Select(x => x.ErrorMessage).ToArray();
                return _vrs.Count > 0;
            }
            /// <summary>
            /// Verilen JSON dizisini belirli bir JToken türüne (<see cref="JTokenType"/>) dönüştürmeye çalışır. Dönüşüm başarılı olursa, dönüştürülen değeri döner.
            /// </summary>
            /// <typeparam name="TJToken">Hedef JToken türü.</typeparam>
            /// <param name="json">Dönüştürülmek istenen JSON dizisi.</param>
            /// <param name="jtokentype">Beklenen JToken türü.</param>
            /// <param name="outvalue">Başarılı dönüşümde dönen JToken değeri.</param>
            /// <returns>JSON dizisinin beklenen türe uygun olup olmadığını belirtir; uygun ise <see langword="true"/>, değilse <see langword="false"/> döner.</returns>
            public static bool TryJson<TJToken>(string json, JTokenType jtokentype, out TJToken outvalue) where TJToken : JToken
            {
                try
                {
                    var _jt = JToken.Parse(json.ToStringOrEmpty());
                    var _r = _jt.Type == jtokentype;
                    outvalue = _r ? (TJToken)_jt : default;
                    return _r;
                }
                catch
                {
                    outvalue = default;
                    return false;
                }
            }
            /// <summary>
            /// Verilen e-Posta adresinin geçerli bir MailAddress nesnesine dönüştürülmesini sağlar. Eğer dönüşüm başarılı olursa, MailAddress nesnesini döner.
            /// </summary>
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
            /// <para>
            /// Eğer nesne bir <see cref="IDictionary{String, Object}"/> ise anahtar sözlükte aranır. Eğer normal ya da anonim bir nesne ise reflection ile public özelliklerde aranır.
            /// </para>
            /// </summary>
            /// <typeparam name="TKey">Beklenen değer tipi.</typeparam>
            /// <param name="value">Üzerinde arama yapılacak nesne (sözlük, anonim tip, dinamik nesne vb.).</param>
            /// <param name="key">Erişilmek istenen property ya da sözlük anahtar adı.</param>
            /// <param name="outvalue">Bulunursa değerin <typeparamref name="TKey"/> tipinde sonucu, aksi halde varsayılan değer.</param>
            /// <returns>
            /// Anahtar bulunduysa ve değer istenen tipe dönüştürülebiliyorsa <see langword="true"/>, aksi halde <see langword="false"/>.
            /// </returns>
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
                        var _pi = value.GetType().GetProperty(key, BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);
                        if (_pi != null)
                        {
                            var _pivalue = _pi.GetValue(value);
                            if (_pivalue is TKey _tpi)
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
            /// <param name="phonenumberTR">Kontrol edilecek telefon numarası.</param>
            /// <param name="outvalue">Geçerli olduğu tespit edilen telefon numarası.</param>
            /// <returns>Telefon numarasının geçerli biçimde olup olmadığını belirtir; geçerli ise <see langword="true"/>, değilse <see langword="false"/> döner.</returns>
            public static bool TryPhoneNumberTR(string phonenumberTR, out string outvalue)
            {
                phonenumberTR = new(phonenumberTR.ToStringOrEmpty().Where(x => (x == '+' || Char.IsDigit(x))).ToArray());
                if (phonenumberTR.Length == 14 && phonenumberTR.StartsWith("0090")) { phonenumberTR = phonenumberTR.Substring(4); }
                else if (phonenumberTR.Length == 13 && phonenumberTR.StartsWith("+90")) { phonenumberTR = phonenumberTR.Substring(3); }
                else if (phonenumberTR.Length == 12 && phonenumberTR.StartsWith("90")) { phonenumberTR = phonenumberTR.Substring(2); }
                else if (phonenumberTR.Length == 11 && phonenumberTR[0] == '0') { phonenumberTR = phonenumberTR.Substring(1); }
                var _r = phonenumberTR.Length == 10 && Regex.IsMatch(phonenumberTR, @"^\d+$");
                outvalue = _r ? phonenumberTR : "";
                return _r;
            }
            /// <summary>
            /// Verilen türün Nullable (null değeri alabilen) olup olmadığını kontrol eder. Eğer tür nullable ise, outvalue parametresine nullable olmayan tür atanır.
            /// </summary>
            /// <param name="type">Kontrol edilecek tür.</param>
            /// <param name="outvalue">Nullable olmayan tür, dönüş değeri olarak atanır.</param>
            /// <returns>Tür nullable ise <see langword="true"/>, aksi takdirde <see langword="false"/> döner.</returns>
            public static bool TryTypeIsNullable(Type type, out Type outvalue)
            {
                var _t = (type != null && type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>));
                outvalue = (_t ? type.GenericTypeArguments[0] : type);
                return _t;
            }
            /// <summary>
            /// Verilen adresin geçerli bir URI olup olmadığını kontrol eder. Geçerli bir URI ise, outvalue parametresine URI değeri atanır. URI&#39;nın HTTP veya HTTPS protokolüne sahip olması gerekmektedir.
            /// </summary>
            /// <param name="uristring">Doğrulanacak adres.</param>
            /// <param name="outvalue">Geçerli URI değeri, dönüş değeri olarak atanır.</param>
            /// <returns>Geçerli bir URI ise <see langword="true"/>, aksi takdirde <see langword="false"/> döner.</returns>
            public static bool TryUri(string uristring, out Uri outvalue)
            {
                try { outvalue = (Uri.TryCreate(uristring.ToStringOrEmpty(), UriKind.Absolute, out Uri _uri) && _uri.Scheme.Includes(Uri.UriSchemeHttp, Uri.UriSchemeHttps)) ? _uri : default; }
                catch { outvalue = default; }
                return outvalue != null;
            }
            /// <summary>
            /// Verilen türün (Type) özelliklerinden birincil anahtar (Primary Key) olarak işaretlenmiş olanları bulur. Türün özelliklerini tarar ve <see cref="KeyAttribute"/> ile işaretlenmiş özellikleri döndürür.
            /// </summary>
            /// <param name="type">Kontrol edilecek tür.</param>
            /// <param name="outvalue">Birincil anahtar olarak işaretlenmiş özelliklerin (PropertyInfo) dizisi. Tür null ise boş bir dizi, hata durumunda default döner.</param>
            /// <returns>En az bir birincil anahtar bulunursa <see langword="true"/>, aksi takdirde <see langword="false"/> döner.</returns>
            public static bool TryTableisKeyAttribute(Type type, out PropertyInfo[] outvalue)
            {
                try
                {
                    outvalue = (type == null ? Array.Empty<PropertyInfo>() : type.GetProperties().Where(x => x.IsPK()).ToArray());
                    return outvalue.Length > 0;
                }
                catch
                {
                    outvalue = default;
                    return false;
                }
            }
            /// <summary>
            /// Belirtilen öğe üzerinde verilen türde bir özel niteliğin (attribute) var olup olmadığını kontrol eder. Eğer nitelik bulunursa, <paramref name="outvalue"/> parametresine niteliğin değeri atanır.
            /// </summary>
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
            /// <summary>
            /// Verilen byte dizisinden bir resim (<see cref="Image"/>) oluşturmayı dener. Başarılı olursa, <paramref name="outvalue"/> parametresine oluşturulan resim atanır. Resim nesnesinin kullanımında dikkatli olunmalı ve gerektiğinde dispose edilmelidir.
            /// </summary>
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
            /// <summary>
            /// Verilen değerin geçerli bir MAC adresi olup olmadığını kontrol eder. Eğer geçerliyse, <paramref name="outvalue"/> parametresine temizlenmiş MAC adresi atanır. MAC adresinin belirli bir biçimde olması gerekmektedir.
            /// </summary>
            /// <param name="value">Kontrol edilecek MAC adresi.</param>
            /// <param name="outvalue">Geçerli MAC adresi, dönüş değeri olarak atanır.</param>
            /// <returns>Geçerli bir MAC adresi varsa <see langword="true"/>, aksi takdirde <see langword="false"/> döner.</returns>
            public static bool TryMACAddress(string value, out string outvalue)
            {
                try
                {
                    value = value.ToStringOrEmpty().ToUpper();
                    if (value.Length == _maximumlength.mac && Regex.IsMatch(value, @"^([0-9A-F]{2}[:-]){5}([0-9A-F]{2})$"))
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
                    var _q = (TryUri(value, out Uri _u) && _u.Host.Contains("maps.google.com")) ? (new QueryString(_u.Query).ParseOrDefault<string>("q") ?? "").Split(',') : value.ToStringOrEmpty().Split(',').Select(x => x.ToStringOrEmpty()).Where(x => x != "").ToArray();
                    if (_q.Length == 2 && _q.All(isgooglemapscoordinatecheck_private))
                    {
                        outvalue = new($"https://maps.google.com/?q={String.Join(",", _q)}");
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
            private static bool isgooglemapscoordinatecheck_private(string value) => (Decimal.TryParse(value.Trim(), NumberStyles.AllowDecimalPoint | NumberStyles.AllowLeadingSign, CultureInfo.InvariantCulture, out decimal _r) && _r >= Convert.ToDecimal(-180) && _r <= Convert.ToDecimal(180));
            /// <summary>
            /// Verilen bir metni, belirtilen diller arasında asenkron olarak çevirir.
            /// <para>
            /// Bu metot, çeviri işlemi için Google Çeviri API&#39;sini kullanarak, verilen &quot;value&quot; parametresindeki metni &quot;from&quot; dilinden &quot;to&quot; diline çevirir. Varsayılan olarak &quot;from&quot; dili Türkçe (tr) olarak ayarlanmıştır. Eğer çeviri işlemi başarılı olursa, metnin çevirisi ve işlem durumu döndürülür. Hata durumunda, boş bir değer ve false durumu döner.
            /// </para>
            /// </summary>
            public static async Task<(bool statuswarning, string value)> TryGoogleTranslateAsync(string value, TimeSpan timeout, CancellationToken cancellationtoken = default, string to = "en", string from = "tr")
            {
                value = value.ToStringOrEmpty();
                if (value == "") { return (false, ""); }
                Guard.CheckEmpty(to, nameof(to));
                Guard.CheckEmpty(from, nameof(from));
                try
                {
                    using (var client = new HttpClient
                    {
                        Timeout = timeout,
                        DefaultRequestHeaders = { UserAgent = { new("Mozilla", "4.0") } }
                    })
                    {
                        var _response = await client.GetStringAsync($"https://translate.googleapis.com/translate_a/single?client=gtx&sl={from}&tl={to}&dt=t&q={Uri.EscapeDataString(HttpUtility.HtmlEncode(value))}", cancellationtoken);
                        using (var doc = JsonDocument.Parse(_response))
                        {
                            return (false, doc.RootElement[0][0][0].GetString().ToStringOrEmpty());
                        }
                    }
                }
                catch { return (true, ""); }
            }
        }
    }
}