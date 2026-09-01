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
    using static UD.Core.Helper.GlobalConstants;
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
        private readonly Faker _fakerEN;
        private readonly string _locale;
        private readonly float _nullChange;
        private readonly int _arrayMinLength;
        private readonly int _arrayMaxLength;
        private readonly Dictionary<string, Func<Faker, object?>> _valueStringFactories;
        private byte _minByte = Byte.MinValue, _maxByte = Byte.MaxValue;
        private short _shortMin = 0, _shortMax = Int16.MaxValue;
        private int _intMin = 0, _intMax = Int32.MaxValue;
        private long _longMin = 0, _longMax = Int64.MaxValue;
        private decimal _decimalMin = Decimal.Zero, _decimalMax = Decimal.One;
        private DateTime? _dateTimeMin = null, _dateTimeMax = null;
        private DateOnly? _dateOnlyMin = null, _dateOnlyMax = null;
        private DateTimeOffset? _dateTimeOffsetMin = null, _dateTimeOffsetMax = null;
        private TimeSpan? _timeSpanMin = null, _timeSpanMax = null;
        private TimeOnly? _timeOnlyMin = null, _timeOnlyMax = null;
        /// <summary>Varsayılan yapılandırıcı</summary>
        /// <param name="locale">Kullanılacak yerel ayar (örneğin, &quot;tr&quot; için Türkçe, &quot;en&quot; için İngilizce).</param>
        /// <param name="nullChange">0 ile 1 arasında bir olasılık değeri (0: asla null, 1: her zaman null).</param>
        /// <param name="arrayMinLength">Array türünde propertylerin minimum oluşabileceği eleman sayısı.</param>
        /// <param name="arrayMaxLength">Array türünde propertylerin maksimum oluşabileceği eleman sayısı. Değer 0 olursa [] oluşur</param>
        public BogusGenericFakeDataGenerator(string locale = "tr", float nullChange = 0.25F, int arrayMinLength = 0, int arrayMaxLength = 10)
        {
            this._fakerEN = new("en");
            this._locale = locale;
            this._nullChange = (nullChange > 1 ? 1 : (nullChange < 0 ? 0 : nullChange));
            this._arrayMinLength = arrayMinLength > 0 ? arrayMinLength : 0;
            this._arrayMaxLength = arrayMaxLength > 0 ? arrayMaxLength : 0;
            this._valueStringFactories = new(StringComparer.OrdinalIgnoreCase)
            {
                ["address"] = faker => faker.Address.FullAddress(),
                ["color"] = faker => faker.Internet.Color().ToUpper(),
                ["email"] = faker => this.CreateEMail(faker).Address,
                ["fulladdress"] = faker => faker.Address.FullAddress(),
                ["fullname"] = this.CreateFullName,
                ["ipaddress"] = faker => this.CreateIPAddress().ToString(),
                ["mac"] = faker => faker.Internet.Mac().ToUpper(),
                ["name"] = faker => faker.Person.FirstName,
                ["phone"] = faker => faker.Phone.PhoneNumber("(5##) ###-####"),
                ["seo"] = faker => this.CreateFullName(faker).ToSeoFriendly(),
                ["src"] = _ => this.CreateUri(),
                ["surname"] = faker => faker.Person.LastName.ToUpper(),
                ["uri"] = _ => this.CreateUri(),
                ["username"] = faker => this.CreateEMail(faker).User
            };
        }
        public BogusGenericFakeDataGenerator WithByteRange(byte minByte, byte maxByte)
        {
            this._minByte = minByte;
            this._maxByte = maxByte;
            return this;
        }
        public BogusGenericFakeDataGenerator WithShortRange(short shortMin, short shortMax)
        {
            this._shortMin = shortMin;
            this._shortMax = shortMax;
            return this;
        }
        public BogusGenericFakeDataGenerator WithIntegerRange(int intMin, int intMax)
        {
            this._intMin = intMin;
            this._intMax = intMax;
            return this;
        }
        public BogusGenericFakeDataGenerator WithLongRange(long longMin, long longMax)
        {
            this._longMin = longMin;
            this._longMax = longMax;
            return this;
        }
        public BogusGenericFakeDataGenerator WithDecimalRange(decimal decimalMin, decimal decimalMax)
        {
            this._decimalMin = decimalMin;
            this._decimalMax = decimalMax;
            return this;
        }
        public BogusGenericFakeDataGenerator WithDateTimeRange(DateTime dateTimeMin, DateTime dateTimeMax)
        {
            this._dateTimeMin = dateTimeMin;
            this._dateTimeMax = dateTimeMax;
            return this;
        }
        public BogusGenericFakeDataGenerator WithDateOnlyRange(DateOnly dateOnlyMin, DateOnly dateOnlyMax)
        {
            this._dateOnlyMin = dateOnlyMin;
            this._dateOnlyMax = dateOnlyMax;
            return this;
        }
        public BogusGenericFakeDataGenerator WithDateTimeOffsetRange(DateTimeOffset dateTimeOffsetMin, DateTimeOffset dateTimeOffsetMax)
        {
            this._dateTimeOffsetMin = dateTimeOffsetMin;
            this._dateTimeOffsetMax = dateTimeOffsetMax;
            return this;
        }
        public BogusGenericFakeDataGenerator WithTimeSpanRange(TimeSpan timeSpanMin, TimeSpan timeSpanMax)
        {
            this._timeSpanMin = timeSpanMin;
            this._timeSpanMax = timeSpanMax;
            return this;
        }
        public BogusGenericFakeDataGenerator WithTimeOnlyRange(TimeOnly timeOnlyMin, TimeOnly timeOnlyMax)
        {
            this._timeOnlyMin = timeOnlyMin;
            this._timeOnlyMax = timeOnlyMax;
            return this;
        }
        public T Generate<T>() where T : class => this.GenerateArray<T>(1)[0];
        public T[] GenerateArray<T>(int count) where T : class
        {
            if (count > 0) { return new Faker<T>(this._locale).CustomInstantiator(f => (T)this.CreateFakeInstance("", typeof(T), f)).Generate(count).ToArray(); }
            return [];
        }
        #region Private Methods
        private string CreateUri() => this._fakerEN.Internet.Url().TrimEnd('/');
        private string CreateFullName(Faker faker) => String.Concat(faker.Person.FirstName, " ", faker.Person.LastName.ToUpper()).Trim();
        private MailAddress CreateEMail(Faker faker) => new(this._fakerEN.Internet.ExampleEmail().ToLower(), CreateFullName(faker));
        private IPAddress CreateIPAddress() => this._fakerEN.Internet.IpAddress().MapToIPv4();
        private static bool IsEqual(string parameterName, string value) => parameterName.Equals(value, StringComparison.OrdinalIgnoreCase);
        private static int GetSignificantDigits(Faker faker) => (MaximumLengthConstants.TRTaxIdentityNumber - (faker.Random.Bool(0.9f) ? 0 : (faker.Random.Bool(0.9f) ? 1 : 2)));
        private object CreateFakeInstance(string parameterName, Type type, Faker faker)
        {
            if (TryValidators.TryTypeIsNullable(type, out Type _baseType)) { return faker.Random.Bool(this._nullChange) ? null : this.CreateFakeInstance(parameterName, _baseType, faker); }
            if (type == typeof(string))
            {
                if (this._valueStringFactories.TryGetValue(parameterName, out var factory)) { return factory(faker); }
                return faker.Commerce.ProductName();
            }
            if (type.IsEnum) { return faker.PickRandom(Enum.GetValues(type)); }
            if (type == typeof(byte)) { return faker.Random.Byte(this._minByte, this._maxByte); }
            if (type == typeof(short))
            {
                if (IsEqual(parameterName, "internal")) { return faker.Random.Short(1000, 9999); }
                return faker.Random.Short(this._shortMin, this._shortMax);
            }
            if (type == typeof(int)) { return faker.Random.Int(this._intMin, this._intMax); }
            if (type == typeof(long))
            {
                if (IsEqual(parameterName, "tridentitynumber")) { return Generator.FakeTRIdentityNumber(); }
                if (IsEqual(parameterName, "trtaxidentitynumber")) { return Generator.FakeTRTaxIdentityNumber(GetSignificantDigits(faker)); }
                return faker.Random.Long(this._longMin, this._longMax);
            }
            if (type == typeof(bool)) { return faker.Random.Bool(); }
            if (type == typeof(decimal)) { return faker.Random.Decimal(this._decimalMin, this._decimalMax); }
            if (type == typeof(Guid)) { return faker.Random.Guid(); }
            if (type == typeof(DateTime)) { return ((this._dateTimeMin.HasValue && this._dateTimeMax.HasValue) ? faker.Date.Between(this._dateTimeMin.Value, this._dateTimeMax.Value) : faker.Date.Past()); }
            if (type == typeof(DateOnly)) { return ((this._dateOnlyMin.HasValue && this._dateOnlyMax.HasValue) ? faker.Date.BetweenDateOnly(this._dateOnlyMin.Value, this._dateOnlyMax.Value) : faker.Date.PastDateOnly()); }
            if (type == typeof(DateTimeOffset)) { return ((this._dateTimeOffsetMin.HasValue && this._dateTimeOffsetMax.HasValue) ? faker.Date.BetweenOffset(this._dateTimeOffsetMin.Value, this._dateTimeOffsetMax.Value) : faker.Date.PastOffset()); }
            if (type == typeof(TimeSpan))
            {
                if (this._timeSpanMin.HasValue && this._timeSpanMax.HasValue)
                {
                    var ticksRange = this._timeSpanMax.Value.Ticks - this._timeSpanMin.Value.Ticks;
                    return TimeSpan.FromTicks(this._timeSpanMin.Value.Ticks + faker.Random.Long(0, ticksRange));
                }
                return faker.Date.Timespan();
            }
            if (type == typeof(TimeOnly)) { return ((this._timeOnlyMin.HasValue && this._timeOnlyMax.HasValue) ? faker.Date.BetweenTimeOnly(this._timeOnlyMin.Value, this._timeOnlyMax.Value) : faker.Date.RecentTimeOnly()); }
            if (type == typeof(Uri)) { return new Uri(this.CreateUri()); }
            if (type == typeof(MailAddress)) { return this.CreateEMail(faker); }
            if (type == typeof(IPAddress)) { return this.CreateIPAddress(); }
            if (type.IsArray)
            {
                int i, count = (this._arrayMaxLength > 0 ? faker.Random.Int(this._arrayMinLength, this._arrayMaxLength) : 0);
                var elementType = type.GetElementType();
                var array = Array.CreateInstance(elementType, count);
                for (i = 0; i < count; i++) { array.SetValue(this.CreateFakeInstance(parameterName, elementType, faker), i); }
                return array;
            }
            if (type.IsGenericType)
            {
                var definingType = type.GetGenericTypeDefinition();
                if (definingType == typeof(Dictionary<,>))
                {
                    var keyType = type.GetGenericArguments()[0];
                    var valueType = type.GetGenericArguments()[1];
                    int i, count = (this._arrayMaxLength > 0 ? faker.Random.Int(this._arrayMinLength, this._arrayMaxLength) : 0);
                    var dict = (IDictionary)Activator.CreateInstance(type);
                    for (i = 0; i < count; i++)
                    {
                        var key = this.CreateFakeInstance(parameterName, keyType, faker);
                        if (dict.Contains(key)) { continue; }
                        dict.Add(key, this.CreateFakeInstance(parameterName, valueType, faker));
                    }
                    return dict;
                }
                if (definingType == typeof(List<>))
                {
                    var elementType = type.GetGenericArguments()[0];
                    int i, count = (this._arrayMaxLength > 0 ? faker.Random.Int(this._arrayMinLength, this._arrayMaxLength) : 0);
                    var list = (IList)Activator.CreateInstance(type);
                    for (i = 0; i < count; i++) { list.Add(this.CreateFakeInstance(parameterName, elementType, faker)); }
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
                var args = ctor.GetParameters().Select(x => this.CreateFakeInstance(x.Name, x.ParameterType, faker)).ToArray();
                return ctor.Invoke(args);
            }
            return null;
        }
        #endregion
    }
}