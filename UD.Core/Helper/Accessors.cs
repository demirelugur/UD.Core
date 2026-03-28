namespace UD.Core.Helper
{
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.StaticFiles;
    using Microsoft.Data.SqlClient;
    using Newtonsoft.Json.Linq;
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Data;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Security.Cryptography;
    using UD.Core.Extensions;
    using UD.Core.Helper.Validation;
    using static UD.Core.Helper.GlobalConstants;
    public sealed class Accessors
    {
        /// <summary>
        /// Veritabanı bağlantı dizesi oluşturur. 
        /// <para><c>Data Source=###;Initial Catalog=###;Persist Security Info=True;User ID=###;Password=###;MultipleActiveResultSets=True;TrustServerCertificate=True</c></para>
        /// </summary>
        /// <param name="dataSource">Veritabanı sunucu adı veya IP adresi (DataSource)</param>
        /// <param name="initialCatalog">Bağlanılacak veritabanı adı (Initial Catalog)</param>
        /// <param name="userID">Veritabanı kullanıcı adı (User ID)</param>
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
        public static string GetConnectionString(string dataSource, string initialCatalog, string userID, string password)
        {
            Guard.ThrowIfEmpty(dataSource, nameof(dataSource));
            Guard.ThrowIfEmpty(initialCatalog, nameof(initialCatalog));
            Guard.ThrowIfEmpty(userID, nameof(userID));
            Guard.ThrowIfEmpty(password, nameof(password));
            return new SqlConnectionStringBuilder
            {
                DataSource = dataSource,
                InitialCatalog = initialCatalog,
                UserID = userID,
                Password = password,
                PersistSecurityInfo = true,
                MultipleActiveResultSets = true,
                TrustServerCertificate = true
            }.ToString();
        }
        /// <summary>Verilen sınıfın belirtilen özelliğindeki maksimum karakter uzunluğunu döner. Eğer <see cref="StringLengthAttribute"/> veya <see cref="MaxLengthAttribute"/> gibi uzunluk sınırlayıcı öznitelikler atanmışsa, bu değeri alır. Aksi takdirde 0 döner.</summary>
        /// <typeparam name="T">Kontrol edilecek sınıf türü.</typeparam>
        /// <param name="name">Kontrol edilecek özelliğin adı.</param>
        /// <returns>Özellik için maksimum uzunluk değeri; öznitelik bulunmazsa 0 döner.</returns>
        public static int GetStringOrMaxLength<T>(string name) where T : class
        {
            Guard.ThrowIfEmpty(name, nameof(name));
            return typeof(T).GetProperty(name).GetStringOrMaxLength();
        }
        /// <summary>Verilen sınıfın belirli bir string ifadesi için maksimum karakter uzunluğunu döner. <see cref="StringLengthAttribute"/> veya <see cref="MaxLengthAttribute"/> atanmışsa bu değeri alır, aksi takdirde 0 döner.</summary>
        /// <typeparam name="T">Kontrol edilecek sınıf türü.</typeparam>
        /// <param name="expression">Özellik ismini içeren ifade.</param>
        /// <returns>Özellik için maksimum uzunluk değeri; öznitelik bulunmazsa 0 döner.</returns>
        public static int GetStringOrMaxLength<T>(Expression<Func<T, string>> expression) where T : class => GetStringOrMaxLength<T>(expression.GetExpressionName());
        /// <summary>
        /// Verilen string dizisinden kısaltma oluşturur. Her bir kelimenin baş harfini alarak noktalarla ayrılmış bir kısaltma döner. Boş veya null değerler atlanır.
        /// <example><br />Örnek: <br />Uğur DEMİREL -> U.D <br />Mustafa Kemal ATATÜRK -> MK.A</example>
        /// </summary>
        /// <param name="names">Kısaltma için kullanılacak isim dizisi.</param>
        /// <returns>Verilen isimlerin baş harflerinden oluşan kısaltma. Eğer parametreler boş veya geçersizse boş string döner.</returns>
        public static string GetNameInitials(params string[] names)
        {
            names = (names ?? []).Select(x => x.ToStringOrEmpty()).Where(x => x != "").ToArray();
            if (names.Length == 0) { return ""; }
            return String.Join(".", names.Select(x => String.Join("", x.ToUpper().Split([' '], StringSplitOptions.RemoveEmptyEntries).Select(x => x[0]).ToArray())));
        }
        /// <summary>Verilen nesneden &quot;isldate&quot; ve &quot;isluser&quot; bilgilerini çıkarır ve bu bilgileri belirtilen tarih formatında birleştirerek string olarak döndürür. Nesne tipi IFormCollection, JToken, Tuple, ValueTuple veya bir enumerable koleksiyon olabilir. Eğer nesne null ise, &quot;isldate&quot; geçerli bir tarih değilse veya &quot;isluser&quot; boşsa, boş string döndürür.</summary>
        /// <param name="model">Bilgilerin alınacağı nesne.</param>
        /// <param name="dateFormat">Tarih biçimi.</param>
        /// <returns>&quot;isldate&quot; ve &quot;isluser&quot; bilgilerinin birleştirilmiş string hali; geçerli veri yoksa boş string.</returns>
        public static string GetIslemInfoFromObject(object model, string dateFormat = DateConstants.ddMMyyyy_HHmmss)
        {
            if (model == null) { return ""; }
            if (model is IFormCollection _form)
            {
                return GetIslemInfoFromObject(new
                {
                    isldate = _form.ParseOrDefault<DateTime>("isldate"),
                    isluser = _form.ParseOrDefault<string>("isluser") ?? ""
                }, dateFormat);
            }
            if (model is JToken _j)
            {
                return GetIslemInfoFromObject(new
                {
                    isldate = _j["isldate"].ToEnumerable().Select(x => (x != null && x.Type == JTokenType.Date) ? Convert.ToDateTime(x) : DateTime.MinValue).FirstOrDefault(),
                    isluser = _j["isluser"].ToEnumerable().Select(x => (x != null && x.Type == JTokenType.String) ? Convert.ToString(x) : "").FirstOrDefault()
                }, dateFormat);
            }
            var type = model.GetType();
            if (type.FullName.StartsWith("System.Tuple") || type.FullName.StartsWith("System.ValueTuple"))
            {
                var args = type.GetGenericArguments();
                if (args.Length >= 2)
                {
                    var types = new[] { typeof(DateTime), typeof(DateOnly), typeof(DateTimeOffset) };
                    if (types.Contains(args[0]) && args[1] == typeof(string))
                    {
                        dynamic t = model;
                        return GetIslemInfoFromObject(new
                        {
                            isldate = (object)t.Item1,
                            isluser = (string)t.Item2
                        }, dateFormat);
                    }
                    if (args[0] == typeof(string) && types.Contains(args[1]))
                    {
                        dynamic t = model;
                        return GetIslemInfoFromObject(new
                        {
                            isldate = (object)t.Item2,
                            isluser = (string)t.Item1
                        }, dateFormat);
                    }
                }
            }
            return model.ToEnumerable().Select(x => x.ToDynamic()).Select(x => new
            {
                isldate = Converters.ToDateTimeFromObject((object)x.isldate, default),
                isluser = (string)x.isluser
            }).Select(x => String.Join(", ", new string[] { (x.isldate.Ticks > 0 ? x.isldate.ToString(dateFormat) : ""), x.isluser.ToStringOrEmpty() }.Where(y => y != "").ToArray())).FirstOrDefault() ?? "";
        }
        /// <summary>Sadece uzantıdan MIME type döner. Eğer herhangi bir kayıt bulunamazsa &quot;application/octet-stream&quot; değerini döner</summary>
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
        /// <summary>Belirtilen uzunlukta, kriptografik olarak güvenli rastgele bayt dizisi (anahtar) üretir. </summary>
        /// <param name="length">Üretilecek anahtarın bayt cinsinden uzunluğu.</param>
        /// <returns>Rastgele üretilmiş baytlardan oluşan anahtar dizisi.</returns>
        public static byte[] GenerateRandomKey(int length)
        {
            Guard.ThrowIfZeroOrNegative(length, nameof(length));
            using (var rng = RandomNumberGenerator.Create())
            {
                var byteArray = new byte[length];
                rng.GetBytes(byteArray);
                return byteArray;
            }
        }
        /// <summary>Metni belirtilen maksimum uzunluğa kadar kısaltır. Metin belirtilen uzunluğu aşıyorsa sonuna üç nokta (...) ekler. Metin boş veya null ise boş string döner. </summary>
        /// <param name="value">İşlem yapılacak metin</param>
        /// <param name="maxLength">3 nokta dahil metnin maksimum uzunluğu</param>
        /// <returns>Kısaltılmış ve gerekiyorsa üç nokta eklenmiş metin</returns>
        public static string SubstringUpToLengthWithEllipsis(string value, int maxLength)
        {
            value = value.SubstringUpToLength(maxLength - 3);
            if (value == "") { return ""; }
            return String.Concat(value, "...");
        }
        /// <summary>Verilen metindeki Türkçe özel karakterleri (Ç, ç, Ğ, ğ, İ, ı, Ö, ö, Ş, ş, Ü, ü) karşılık gelen İngilizce karakterlerle (C, c, G, g, I, i, O, o, S, s, U, u) değiştirir. Eğer metin null ise, boş bir string döner.</summary>
        /// <param name="value">Türkçe karakterlerin değiştirileceği metin.</param>
        /// <returns>Değiştirilmiş metni döner.</returns>
        public static string ReplaceTurkishChars(string value) => value.ToStringOrEmpty().Replace('Ç', 'C').Replace('ç', 'c').Replace('Ğ', 'G').Replace('ğ', 'g').Replace('İ', 'I').Replace('ı', 'i').Replace('Ö', 'O').Replace('ö', 'o').Replace('Ş', 'S').Replace('ş', 's').Replace('Ü', 'U').Replace('ü', 'u');
    }
}