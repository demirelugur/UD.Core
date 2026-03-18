namespace UD.Core.Extensions
{
    using System;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;
    using UD.Core.Helper.Results;
    using UD.Core.Helper.Validation;
    using static UD.Core.Helper.OrtakTools;
    public static class TypeExtensions
    {
        /// <summary>Verilen türün (Type) bir tabloya eşlendiğini kontrol eder. Türün, <see cref="TableAttribute"/> ile işaretlenmiş olup olmadığını kontrol ederek tabloya eşlenip eşlenmediğini döndürür.</summary>
        /// <param name="type">Kontrol edilecek tür.</param>
        /// <returns>Tür bir tabloya eşlenmişse <see langword="true"/>, değilse <see langword="false"/> döner.</returns>
        public static bool IsMappedTable(this Type type)
        {
            Guard.ThrowIfNull(type, nameof(type));
            return Validators.TryCustomAttribute(type, out TableAttribute _);
        }
        /// <summary>Belirtilen türün nullable olup olmadığını kontrol eder.</summary>
        /// <param name="type">Kontrol edilecek tür.</param>
        /// <returns>Nullable ise <see langword="true"/>, değilse <see langword="false"/> döner.</returns>
        public static bool IsNullable(this Type type)
        {
            Guard.ThrowIfNull(type, nameof(type));
            return Validators.TryTypeIsNullable(type, out _);
        }
        /// <summary>Belirtilen türün özel bir sınıf olup olmadığını kontrol eder.</summary>
        /// <param name="type">Kontrol edilecek tür.</param>
        /// <returns>Özel sınıf ise <see langword="true"/>, değilse <see langword="false"/> döner.</returns>
        public static bool IsCustomClass(this Type type)
        {
            Guard.ThrowIfNull(type, nameof(type));
            return (type.IsClass && type != typeof(string) && !type.IsArray && !typeof(Delegate).IsAssignableFrom(type) && !type.IsInterface);
        }
        /// <summary>Belirtilen <see cref="Type"/> için tanımlı <see cref="TableAttribute"/> özniteliğini kullanarak tablo adını döndürür. Varsayılan olarak şema adı &quot;dbo&quot; kabul edilir. <paramref name="isSquareBrackets"/> true ise tablo ve şema adları köşeli parantez içerisine alınır.</summary>
        /// <param name="type">Tabloya karşılık gelen sınıf tipi.</param>
        /// <param name="isSquareBrackets">Tablo ve şema adlarının köşeli parantez içerisine alınıp alınmayacağını belirtir.</param>
        /// <returns>Şema ve tablo adını içeren tam tablo adı.</returns>
        /// <exception cref="NotSupportedException">Eğer belirtilen tip üzerinde <see cref="TableAttribute"/> özniteliği bulunmazsa fırlatılır.</exception>
        public static string GetTableName(this Type type, bool isSquareBrackets)
        {
            Guard.ThrowIfNull(type, nameof(type));
            if (Validators.TryCustomAttribute(type, out TableAttribute _ta))
            {
                Guard.ThrowIfEmpty(_ta.Name, nameof(_ta.Name));
                var r = new List<string> { _ta.Schema.CoalesceOrDefault("dbo"), _ta.Name };
                if (isSquareBrackets) { return String.Join(".", r.Select(x => $"[{x}]").ToArray()); }
                return String.Join(".", r);
            }
            if (Guards.IsEnglishDefaultThreadCurrentUICulture) { throw new NotSupportedException($"The type \"{type.FullName}\" does not have the \"{typeof(TableAttribute).FullName}\" attribute. ", new Exception("To get the table name, the relevant class must be decorated with the [Table(\"TableName\")] attribute.")); }
            throw new NotSupportedException($"\"{type.FullName}\" tipi üzerinde \"{typeof(TableAttribute).FullName}\" özniteliği bulunmamaktadır. ", new Exception("Tablo adını alabilmek için ilgili sınıfa [Table(\"TabloAdi\")] özniteliği eklenmelidir."));
        }
        /// <summary>Belirtilen türü enum dizisine dönüştürür.</summary>
        /// <param name="type">Enum türü.</param>
        /// <returns>Enum sonuçları dizisi.</returns>
        public static EnumResult[] ToEnumArray(this Type type)
        {
            Guard.ThrowIfNull(type, nameof(type));
            if (!type.IsEnum)
            {
                if (Guards.IsEnglishDefaultThreadCurrentUICulture) { throw new ArgumentException($"The type \"{type.FullName}\" must be a valid \"{nameof(Enum)}\" type!", nameof(type)); }
                throw new ArgumentException($"\"{type.FullName}\" türü geçerli bir \"{nameof(Enum)}\" türü olmalıdır!", nameof(type));
            }
            var values = Enum.GetValues(type);
            return Enum.GetNames(type).Select((enumName, i) => new EnumResult(Convert.ToInt64(values.GetValue(i)), enumName, type.GetField(enumName).GetDescription())).ToArray();
        }
        /// <summary> Belirtilen tipin, verilen açık generic interface'i (open generic interface) implement edip etmediğini kontrol eder.</summary>
        /// <param name="type">Kontrol edilecek tip (class, struct, record vs.)</param>
        /// <param name="openGenericInterface">Açık generic interface tanımı</param>
        public static bool IsImplementsOpenGenericInterface(this Type type, Type openGenericInterface)
        {
            Guard.ThrowIfNull(type, nameof(type));
            Guard.ThrowIfNull(openGenericInterface, nameof(openGenericInterface));
            return (type.GetInterfaces().Any(x => x.IsGenericType && x.GetGenericTypeDefinition() == openGenericInterface));
        }
        /// <summary>Belirtilen <paramref name="type"/> türünün, verilen açık generic (<paramref name="openGeneric"/>) türünden türeyip türemediğini kontrol eder.</summary>
        /// <param name="type">Kontrol edilecek tür.</param>
        /// <param name="openGeneric">Açık generic taban türü.</param>
        public static bool IsSubclassOfOpenGeneric(this Type type, Type openGeneric)
        {
            Guard.ThrowIfNull(type, nameof(type));
            Guard.ThrowIfNull(openGeneric, nameof(openGeneric));
            while (type != typeof(object))
            {
                var cur = (type.IsGenericType ? type.GetGenericTypeDefinition() : type);
                if (cur == openGeneric) { return true; }
                type = type.BaseType;
            }
            return false;
        }
        /// <summary>Belirtilen enum türündeki değerleri ve karşılık gelen long değerlerini içeren bir sözlük oluşturur.</summary>
        /// <param name="type">Enum türü.</param>
        /// <returns>Enum isimlerini ve long karşılıklarını içeren sözlük.</returns>
        public static Dictionary<string, long> ToDictionaryFromEnum(this Type type)
        {
            Guard.ThrowIfNull(type, nameof(type));
            if (!type.IsEnum) { return []; }
            var values = Enum.GetValues(type);
            var names = Enum.GetNames(type);
            int i, vl = values.Length;
            var dict = new Dictionary<string, long>(vl);
            for (i = 0; i < vl; i++) { dict.Add(names[i], Convert.ToInt64(values.GetValue(i))); }
            return dict;
        }
    }
}