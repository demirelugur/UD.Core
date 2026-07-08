namespace UD.Core.Helper.Configurations
{
    using Bogus;
    using System;
    using System.Collections;
    using System.Linq;
    using System.Net;
    using System.Net.Mail;
    using UD.Core.Extensions;
    using UD.Core.Helper;
    using UD.Core.Helper.Generates;
    /// <summary>
    /// Sahte veri üretimi için kullanılan genel bir sınıf. Bogus kütüphanesini kullanarak farklı veri türlerinde özelleştirilebilir sahte veriler üretir.
    /// <list type="bullet">
    /// <item>String için özel işaretlenmiş property adları: <c>address,color,email,fulladdress,fullname,ipaddress,mac,name,phone,seo,src,surname,uri,username</c></item>
    /// <item>Int16 için özel işaretlenmiş property adları: <c>internal</c></item>
    /// <item>Int64 için özel işaretlenmiş property adları: <c>tridentitynumber, trtaxidentitynumber</c></item>
    /// </list>
    /// </summary>
    public sealed class BogusGenericFakeDataGenerator
    {
        private readonly Faker fakerEN;
        private readonly string locale;
        private readonly float nullChange;
        private readonly int arrayMinLength;
        private readonly int arrayMaxLength;
        private readonly Dictionary<string, Func<Faker, object?>> valueStringFactories;
        private byte minByte = Byte.MinValue, maxByte = Byte.MaxValue;
        private short shortMin = 0, shortMax = Int16.MaxValue;
        private int intMin = 0, intMax = Int32.MaxValue;
        private long longMin = 0, longMax = Int64.MaxValue;
        private decimal decimalMin = Decimal.Zero, decimalMax = Decimal.One;
        private DateTime? dateTimeMin = null, dateTimeMax = null;
        private DateOnly? dateOnlyMin = null, dateOnlyMax = null;
        private DateTimeOffset? dateTimeOffsetMin = null, dateTimeOffsetMax = null;
        private TimeSpan? timeSpanMin = null, timeSpanMax = null;
        private TimeOnly? timeOnlyMin = null, timeOnlyMax = null;
        /// <summary>Varsayılan yapılandırıcı</summary>
        /// <param name="locale">Kullanılacak yerel ayar (örneğin, &quot;tr&quot; için Türkçe, &quot;en&quot; için İngilizce).</param>
        /// <param name="nullChange">0 ile 1 arasında bir olasılık değeri (0: asla null, 1: her zaman null).</param>
        /// <param name="arrayMinLength">Array türünde propertylerin minimum oluşabileceği eleman sayısı.</param>
        /// <param name="arrayMaxLength">Array türünde propertylerin maksimum oluşabileceği eleman sayısı. Değer 0 olursa [] oluşur</param>
        public BogusGenericFakeDataGenerator(string locale = "tr", float nullChange = 0.25F, int arrayMinLength = 0, int arrayMaxLength = 10)
        {
            this.fakerEN = new("en");
            this.locale = locale;
            this.nullChange = (nullChange > 1 ? 1 : (nullChange < 0 ? 0 : nullChange));
            this.arrayMinLength = arrayMinLength > 0 ? arrayMinLength : 0;
            this.arrayMaxLength = arrayMaxLength > 0 ? arrayMaxLength : 0;
            this.valueStringFactories = new(StringComparer.OrdinalIgnoreCase)
            {
                ["address"] = faker => faker.Address.FullAddress(),
                ["color"] = faker => faker.Internet.Color().ToUpper(),
                ["email"] = faker => this.createEMail(faker).Address,
                ["fulladdress"] = faker => faker.Address.FullAddress(),
                ["fullname"] = faker => createFullName(faker),
                ["ipaddress"] = faker => this.createIPAdress().ToString(),
                ["mac"] = faker => faker.Internet.Mac().ToUpper(),
                ["name"] = faker => faker.Person.FirstName,
                ["phone"] = faker => faker.Phone.PhoneNumber("(5##) ###-####"),
                ["seo"] = faker => createFullName(faker).ToSeoFriendly(),
                ["src"] = _ => this.createUri(),
                ["surname"] = faker => faker.Person.LastName.ToUpper(),
                ["uri"] = _ => this.createUri(),
                ["username"] = faker => this.createEMail(faker).User
            };
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
        public BogusGenericFakeDataGenerator WithDateTimeOffsetRange(DateTimeOffset dateTimeOffsetMin, DateTimeOffset dateTimeOffsetMax)
        {
            this.dateTimeOffsetMin = dateTimeOffsetMin;
            this.dateTimeOffsetMax = dateTimeOffsetMax;
            return this;
        }
        public BogusGenericFakeDataGenerator WithTimeSpanRange(TimeSpan timeSpanMin, TimeSpan timeSpanMax)
        {
            this.timeSpanMin = timeSpanMin;
            this.timeSpanMax = timeSpanMax;
            return this;
        }
        public BogusGenericFakeDataGenerator WithTimeOnlyRange(TimeOnly timeOnlyMin, TimeOnly timeOnlyMax)
        {
            this.timeOnlyMin = timeOnlyMin;
            this.timeOnlyMax = timeOnlyMax;
            return this;
        }
        public T Generate<T>() where T : class => this.GenerateArray<T>(1)[0];
        public T[] GenerateArray<T>(int count) where T : class
        {
            if (count > 0) { return new Faker<T>(this.locale).CustomInstantiator(f => (T)this.createFakeInstance("", typeof(T), f)).Generate(count).ToArray(); }
            return [];
        }
        #region Private Methods
        private string createUri() => this.fakerEN.Internet.Url().TrimEnd('/');
        private static string createFullName(Faker faker) => String.Concat(faker.Person.FirstName, " ", faker.Person.LastName.ToUpper()).Trim();
        private MailAddress createEMail(Faker faker) => new(this.fakerEN.Internet.ExampleEmail().ToLower(), createFullName(faker));
        private IPAddress createIPAdress() => this.fakerEN.Internet.IpAddress().MapToIPv4();
        private bool isEqual(string parameterName, string value) => parameterName.Equals(value, StringComparison.OrdinalIgnoreCase);
        private int getSignificantDigits(Faker faker) => (faker.Random.Bool(0.95f) ? 10 : faker.Random.ArrayElement([8, 9]));
        private object createFakeInstance(string parameterName, Type type, Faker faker)
        {
            if (TryValidators.TryTypeIsNullable(type, out Type _baseType)) { return faker.Random.Bool(this.nullChange) ? null : this.createFakeInstance(parameterName, _baseType, faker); }
            if (type == typeof(string))
            {
                if (this.valueStringFactories.TryGetValue(parameterName, out var factory)) { return factory(faker); }
                return faker.Commerce.ProductName();
            }
            if (type.IsEnum) { return faker.PickRandom(Enum.GetValues(type)); }
            if (type == typeof(byte)) { return faker.Random.Byte(this.minByte, this.maxByte); }
            if (type == typeof(short))
            {
                if (isEqual(parameterName, "internal")) { return faker.Random.Short(1000, 9999); }
                return faker.Random.Short(this.shortMin, this.shortMax);
            }
            if (type == typeof(int)) { return faker.Random.Int(this.intMin, this.intMax); }
            if (type == typeof(long))
            {
                if (isEqual(parameterName, "tridentitynumber")) { return Generator.FakeTRIdentityNumber(); }
                if (isEqual(parameterName, "trtaxidentitynumber")) { return Generator.FakeTRTaxIdentityNumber(this.getSignificantDigits(faker)); }
                return faker.Random.Long(this.longMin, this.longMax);
            }
            if (type == typeof(bool)) { return faker.Random.Bool(); }
            if (type == typeof(decimal)) { return faker.Random.Decimal(this.decimalMin, this.decimalMax); }
            if (type == typeof(Guid)) { return faker.Random.Guid(); }
            if (type == typeof(DateTime)) { return ((this.dateTimeMin.HasValue && this.dateTimeMax.HasValue) ? faker.Date.Between(this.dateTimeMin.Value, this.dateTimeMax.Value) : faker.Date.Past()); }
            if (type == typeof(DateOnly)) { return ((this.dateOnlyMin.HasValue && this.dateOnlyMax.HasValue) ? faker.Date.BetweenDateOnly(this.dateOnlyMin.Value, this.dateOnlyMax.Value) : faker.Date.PastDateOnly()); }
            if (type == typeof(DateTimeOffset)) { return ((this.dateTimeOffsetMin.HasValue && this.dateTimeOffsetMax.HasValue) ? faker.Date.BetweenOffset(this.dateTimeOffsetMin.Value, this.dateTimeOffsetMax.Value) : faker.Date.PastOffset()); }
            if (type == typeof(TimeSpan))
            {
                if (this.timeSpanMin.HasValue && this.timeSpanMax.HasValue)
                {
                    var ticksRange = this.timeSpanMax.Value.Ticks - this.timeSpanMin.Value.Ticks;
                    return TimeSpan.FromTicks(this.timeSpanMin.Value.Ticks + faker.Random.Long(0, ticksRange));
                }
                return faker.Date.Timespan();
            }
            if (type == typeof(TimeOnly)) { return ((this.timeOnlyMin.HasValue && this.timeOnlyMax.HasValue) ? faker.Date.BetweenTimeOnly(this.timeOnlyMin.Value, this.timeOnlyMax.Value) : faker.Date.RecentTimeOnly()); }
            if (type == typeof(Uri)) { return new Uri(this.createUri()); }
            if (type == typeof(MailAddress)) { return this.createEMail(faker); }
            if (type == typeof(IPAddress)) { return this.createIPAdress(); }
            if (type.IsArray)
            {
                int i, count = (this.arrayMaxLength > 0 ? faker.Random.Int(this.arrayMinLength, this.arrayMaxLength) : 0);
                var elementType = type.GetElementType();
                var array = Array.CreateInstance(elementType, count);
                for (i = 0; i < count; i++) { array.SetValue(this.createFakeInstance(parameterName, elementType, faker), i); }
                return array;
            }
            if (type.IsGenericType)
            {
                var definingType = type.GetGenericTypeDefinition();
                if (definingType == typeof(Dictionary<,>))
                {
                    var keyType = type.GetGenericArguments()[0];
                    var valueType = type.GetGenericArguments()[1];
                    int i, count = (this.arrayMaxLength > 0 ? faker.Random.Int(this.arrayMinLength, this.arrayMaxLength) : 0);
                    var dict = (IDictionary)Activator.CreateInstance(type);
                    for (i = 0; i < count; i++)
                    {
                        var key = this.createFakeInstance(parameterName, keyType, faker);
                        if (dict.Contains(key)) { continue; }
                        dict.Add(key, this.createFakeInstance(parameterName, valueType, faker));
                    }
                    return dict;
                }
                if (definingType == typeof(List<>))
                {
                    var elementType = type.GetGenericArguments()[0];
                    int i, count = (this.arrayMaxLength > 0 ? faker.Random.Int(this.arrayMinLength, this.arrayMaxLength) : 0);
                    var list = (IList)Activator.CreateInstance(type);
                    for (i = 0; i < count; i++) { list.Add(this.createFakeInstance(parameterName, elementType, faker)); }
                    return list;
                }
            }
            if (type.IsClass)
            {
                var ctor = type.GetConstructors().OrderByDescending(c => c.GetParameters().Length).FirstOrDefault();
                if (ctor == null)
                {
                    if (Checks.IsEnglishCurrentUICulture) { throw new InvalidOperationException($"No constructor found for \"{type.FullName}\"!"); }
                    throw new InvalidOperationException($"\"{type.FullName}\" için hiçbir kurucu (Constructors) bulunamadı!");
                }
                var args = ctor.GetParameters().Select(x => this.createFakeInstance(x.Name, x.ParameterType, faker)).ToArray();
                return ctor.Invoke(args);
            }
            return null;
        }
        #endregion
    }
}