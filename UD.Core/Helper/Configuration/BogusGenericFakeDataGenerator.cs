namespace UD.Core.Helper.Configuration
{
    using Bogus;
    using System;
    using System.Collections;
    using System.Linq;
    using System.Net;
    using System.Net.Mail;
    using UD.Core.Extensions;
    using static UD.Core.Helper.OrtakTools;
    /// <summary>
    /// Sahte veri üretimi için kullanılan genel bir sınıf. Bogus kütüphanesini kullanarak farklı veri türlerinde özelleştirilebilir sahte veriler üretir.
    /// <list type="bullet">
    /// <item>
    /// String için özel işaretlenmiş property adları: <c>seo, nms, src, ipaddress, color, mac, tel, adres, ad, name, soyad, surname, kuladi, username, eposta, email</c>
    /// </item>
    /// <item>
    /// Int16(Short) için özel işaretlenmiş property adları: <c>dahili</c>
    /// </item>
    /// <item>
    /// Int64(Long) için özel işaretlenmiş property adları: <c>tckn, vkn</c>
    /// </item>
    /// </list>
    /// </summary>
    public sealed class BogusGenericFakeDataGenerator
    {
        private readonly Faker fakerEN;
        private readonly string locale;
        private readonly float nullChange;
        private readonly int arrayMinLength;
        private readonly int arrayMaxLength;
        private byte minByte = Byte.MinValue, maxByte = Byte.MaxValue;
        private short shortMin = 0, shortMax = Int16.MaxValue;
        private int intMin = 0, intMax = Int32.MaxValue;
        private long longMin = 0, longMax = Int64.MaxValue;
        private decimal decimalMin = Decimal.Zero, decimalMax = Decimal.One;
        private DateTime? dateTimeMin = null, dateTimeMax = null;
        private DateOnly? dateOnlyMin = null, dateOnlyMax = null;
        /// <summary>
        /// Varsayılan yapılandırıcı
        /// </summary>
        /// <param name="locale">Kullanılacak yerel ayar (örneğin, &quot;tr&quot; için Türkçe, &quot;en&quot; için İngilizce).</param>
        /// <param name="nullChange">0 ile 1 arasında bir olasılık değeri (0: asla null, 1: her zaman null).</param>
        /// <param name="arrayMinLength">Array türünde propertylerin minimum oluşabileceği eleman sayısı.</param>
        /// <param name="arrayMaxLength">Array türünde propertylerin maksimum oluşabileceği eleman sayısı. Değer 0 olursa [] oluşur</param>
        public BogusGenericFakeDataGenerator(string locale = "tr", float nullChange = 0.25f, int arrayMinLength = 0, int arrayMaxLength = 10)
        {
            this.fakerEN = new("en");
            this.locale = locale;
            this.nullChange = (nullChange > 1 ? 1 : (nullChange < 0 ? 0 : nullChange));
            this.arrayMinLength = arrayMinLength > 0 ? arrayMinLength : 0;
            this.arrayMaxLength = arrayMaxLength > 0 ? arrayMaxLength : 0;
        }
        public BogusGenericFakeDataGenerator WithByteRange(byte minByte, byte maxByte)
        {
            this.minByte = minByte;
            this.maxByte = maxByte;
            return this;
        }
        public BogusGenericFakeDataGenerator WithShortRange(short shortMin, short shortMax)
        {
            this.shortMin = shortMin;
            this.shortMax = shortMax;
            return this;
        }
        public BogusGenericFakeDataGenerator WithIntegerRange(int intMin, int intMax)
        {
            this.intMin = intMin;
            this.intMax = intMax;
            return this;
        }
        public BogusGenericFakeDataGenerator WithLongRange(long longMin, long longMax)
        {
            this.longMin = longMin;
            this.longMax = longMax;
            return this;
        }
        public BogusGenericFakeDataGenerator WithDecimalRange(decimal decimalMin, decimal decimalMax)
        {
            this.decimalMin = decimalMin;
            this.decimalMax = decimalMax;
            return this;
        }
        public BogusGenericFakeDataGenerator WithDateTimeRange(DateTime dateTimeMin, DateTime dateTimeMax)
        {
            this.dateTimeMin = dateTimeMin;
            this.dateTimeMax = dateTimeMax;
            return this;
        }
        public BogusGenericFakeDataGenerator WithDateOnlyRange(DateOnly dateOnlyMin, DateOnly dateOnlyMax)
        {
            this.dateOnlyMin = dateOnlyMin;
            this.dateOnlyMax = dateOnlyMax;
            return this;
        }
        public T Generate<T>() where T : class => this.GenerateArray<T>(1)[0];
        public T[] GenerateArray<T>(int count) where T : class
        {
            if (count > 0) { return new Faker<T>(this.locale).CustomInstantiator(faker => (T)this.createFakeInstance("", typeof(T), faker)).Generate(count).ToArray(); }
            return [];
        }
        private string createUri() => this.fakerEN.Internet.Url().TrimEnd('/');
        private static string createFullName(Faker faker) => String.Concat(faker.Person.FirstName, " ", faker.Person.LastName.ToUpper());
        private MailAddress createEMail(Faker faker) => new(this.fakerEN.Internet.ExampleEmail().ToLower(), createFullName(faker));
        private IPAddress createIPAdress() => this.fakerEN.Internet.IpAddress().MapToIPv4();
        private object createFakeInstance(string parametername, Type type, Faker faker)
        {
            if (Validators.TryTypeIsNullable(type, out Type _genericbasetype)) { return faker.Random.Bool(this.nullChange) ? null : this.createFakeInstance(parametername, _genericbasetype, faker); }
            if (type == typeof(string))
            {
                if (parametername == "seo") { return createFullName(faker).ToSeoFriendly(); }
                if (parametername == "nms") { return createFullName(faker); }
                if (parametername == "src") { return this.createUri(); }
                if (parametername == "ipaddress") { return this.createIPAdress().ToString(); }
                if (parametername == "color") { return faker.Internet.Color().ToUpper(); }
                if (parametername == "mac") { return faker.Internet.Mac().ToUpper(); }
                if (parametername == "tel") { return faker.Phone.PhoneNumber("(5##) ###-####"); }
                if (parametername == "adres") { return faker.Address.FullAddress(); }
                if (parametername.Includes("ad", "name")) { return faker.Person.FirstName; }
                if (parametername.Includes("soyad", "surname")) { return faker.Person.LastName.ToUpper(); }
                if (parametername.Includes("kuladi", "username")) { return this.createEMail(faker).User; }
                if (parametername.Includes("eposta", "email")) { return this.createEMail(faker).Address; }
                return faker.Commerce.ProductName();
            }
            if (type == typeof(byte)) { return faker.Random.Byte(this.minByte, this.maxByte); }
            if (type == typeof(short))
            {
                if (parametername == "dahili") { return faker.Random.Short(1000, 9999); }
                return faker.Random.Short(this.shortMin, this.shortMax);
            }
            if (type == typeof(int)) { return faker.Random.Int(this.intMin, this.intMax); }
            if (type == typeof(long))
            {
                if (parametername == "tckn") { return faker.Random.ArrayElement([10000000146, 19293160506, 35270291346, 35505252760, 37417041838, 48056596160, 57856397112, 66122384800, 69016478326, 75255184164, 78094801254, 78268733060, 79937798144, 81299907768, 86061923892, 88599002742, 89021372822, 93095299084, 93513339668, 94781067710]); }
                if (parametername == "vkn") { return faker.Random.ArrayElement([33583636, 602883151, 1266516393, 1775916611, 3085865484, 3641323334, 3749934537, 5056541626, 5252719378, 5498069343, 5613060112, 6000479747, 6501266542, 7267046912, 7915288675, 9142970393, 9152251176, 9205280623, 9217990731, 9292694379, 9734899775]); }
                return faker.Random.Long(this.longMin, this.longMax);
            }
            if (type == typeof(bool)) { return faker.Random.Bool(); }
            if (type == typeof(decimal)) { return faker.Random.Decimal(this.decimalMin, this.decimalMax); }
            if (type == typeof(Guid)) { return faker.Random.Guid(); }
            if (type == typeof(DateTime)) { return ((this.dateTimeMin.HasValue && this.dateTimeMax.HasValue) ? faker.Date.Between(this.dateTimeMin.Value, this.dateTimeMax.Value) : faker.Date.Past()); }
            if (type == typeof(DateOnly)) { return ((this.dateOnlyMin.HasValue && this.dateOnlyMax.HasValue) ? faker.Date.BetweenDateOnly(this.dateOnlyMin.Value, this.dateOnlyMax.Value) : faker.Date.PastDateOnly()); }
            if (type == typeof(Uri)) { return new Uri(this.createUri()); }
            if (type == typeof(MailAddress)) { return this.createEMail(faker); }
            if (type == typeof(IPAddress)) { return this.createIPAdress(); }
            if (type.IsEnum) { return faker.PickRandom(Enum.GetValues(type).Cast<object>().ToArray()); }
            if (type.IsArray)
            {
                int i, count = (this.arrayMaxLength > 0 ? faker.Random.Int(this.arrayMinLength, this.arrayMaxLength) : 0);
                var elemtype = type.GetElementType();
                var array = Array.CreateInstance(elemtype, count);
                for (i = 0; i < count; i++) { array.SetValue(this.createFakeInstance(parametername, elemtype, faker), i); }
                return array;
            }
            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Dictionary<,>))
            {
                var keytype = type.GetGenericArguments()[0];
                var valuetype = type.GetGenericArguments()[1];
                int i, count = (this.arrayMaxLength > 0 ? faker.Random.Int(this.arrayMinLength, this.arrayMaxLength) : 0);
                var dict = (IDictionary)Activator.CreateInstance(type);
                for (i = 0; i < count; i++)
                {
                    var key = this.createFakeInstance(parametername, keytype, faker);
                    if (dict.Contains(key)) { continue; }
                    dict.Add(key, this.createFakeInstance(parametername, valuetype, faker));
                }
                return dict;
            }
            if (type.IsClass)
            {
                var ctor = type.GetConstructors().FirstOrDefault();
                if (ctor == null) { throw new InvalidOperationException($"\"{type.FullName}\" için genel bir kurucu (Constructors) bulunamadı!"); }
                var args = ctor.GetParameters().Select(x => this.createFakeInstance(x.Name, x.ParameterType, faker)).ToArray();
                return ctor.Invoke(args);
            }
            return null;
        }
    }
}