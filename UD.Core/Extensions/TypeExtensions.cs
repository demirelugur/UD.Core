namespace UD.Core.Extensions
{
    using System;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;
    using UD.Core.Helper;
    using UD.Core.Helper.Results;
    using static UD.Core.Helper.OrtakTools;
    public static class TypeExtensions
    {
        /// <summary>
        /// Verilen türün (Type) bir tabloya eşlendiğini kontrol eder. Türün, <see cref="TableAttribute"/> ile işaretlenmiş olup olmadığını kontrol ederek tabloya eşlenip eşlenmediğini döndürür.
        /// </summary>
        /// <param name="type">Kontrol edilecek tür.</param>
        /// <returns>Tür bir tabloya eşlenmişse <see langword="true"/>, değilse <see langword="false"/> döner.</returns>
        public static bool IsMappedTable(this Type type) => type != null && _try.TryCustomAttribute(type, out TableAttribute _);
        /// <summary>
        /// Belirtilen türün nullable olup olmadığını kontrol eder.
        /// </summary>
        /// <param name="type">Kontrol edilecek tür.</param>
        /// <returns>Nullable ise <see langword="true"/>, değilse <see langword="false"/> döner.</returns>
        public static bool IsNullable(this Type type) => _try.TryTypeIsNullable(type, out _);
        /// <summary>
        /// Belirtilen türün özel bir sınıf olup olmadığını kontrol eder.
        /// </summary>
        /// <param name="type">Kontrol edilecek tür.</param>
        /// <returns>Özel sınıf ise <see langword="true"/>, değilse <see langword="false"/> döner.</returns>
        public static bool IsCustomClass(this Type type) => (type != null && type.IsClass && type != typeof(string) && !type.IsArray && !typeof(Delegate).IsAssignableFrom(type) && !type.IsInterface);
        /// <summary>
        /// Belirtilen <see cref="Type"/> için tanımlı <see cref="TableAttribute"/> özniteliğini kullanarak tablo adını döndürür. Varsayılan olarak şema adı &quot;dbo&quot; kabul edilir. <paramref name="issquarebrackets"/> true ise tablo ve şema adları köşeli parantez içerisine alınır.
        /// </summary>
        /// <param name="type">Tabloya karşılık gelen sınıf tipi.</param>
        /// <param name="issquarebrackets">Tablo ve şema adlarının köşeli parantez içerisine alınıp alınmayacağını belirtir.</param>
        /// <returns>Şema ve tablo adını içeren tam tablo adı.</returns>
        /// <exception cref="NotSupportedException">Eğer belirtilen tip üzerinde <see cref="TableAttribute"/> özniteliği bulunmazsa fırlatılır.</exception>
        public static string GetTableName(this Type type, bool issquarebrackets)
        {
            Guard.CheckNull(type, nameof(type));
            if (_try.TryCustomAttribute(type, out TableAttribute _ta))
            {
                Guard.CheckEmpty(_ta.Name, nameof(_ta.Name));
                var _r = new List<string> { _ta.Schema.CoalesceOrDefault("dbo"), _ta.Name };
                if (issquarebrackets) { return String.Join(".", _r.Select(x => $"[{x}]").ToArray()); }
                return String.Join(".", _r);
            }
            throw new NotSupportedException($"\"{type.FullName}\" tipi üzerinde \"{typeof(TableAttribute).FullName}\" özniteliği bulunmamaktadır. ", new Exception("Tablo adını alabilmek için ilgili sınıfa [Table(\"TabloAdi\")] özniteliği eklenmelidir."));
        }
        /// <summary>
        /// Belirtilen türü enum dizisine dönüştürür.
        /// </summary>
        /// <param name="type">Enum türü.</param>
        /// <returns>Enum sonuçları dizisi.</returns>
        public static EnumResult[] ToEnumArray(this Type type) => Enum.GetNames(type).Select((enumname, i) => new EnumResult(Convert.ToInt64(Enum.GetValues(type).GetValue(i)), enumname, type.GetField(enumname).GetDescription())).ToArray();
    }
}