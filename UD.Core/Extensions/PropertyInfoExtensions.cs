namespace UD.Core.Extensions
{
    using UD.Core.Helper;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Reflection;
    using static UD.Core.Helper.OrtakTools;
    public static class PropertyInfoExtensions
    {
        /// <summary>
        /// Belirtilen özellik bilgilerinin veritabanı kaynaklı haritalanmış bir özellik olup olmadığını kontrol eder.
        /// </summary>
        /// <param name="pi">Kontrol edilecek özellik bilgisi.</param>
        /// <returns>Haritalanmış bir özellik ise <see langword="true"/>, değilse false <see langword="false"/>.</returns>
        public static bool IsMapped(this PropertyInfo pi)
        {
            if (pi == null) { return false; }
            if (!pi.CanRead || !pi.CanWrite || pi.IsNotMapped()) { return false; }
            if ((pi.GetMethod.IsVirtual || pi.SetMethod.IsVirtual) && (pi.PropertyType.IsMappedTable() || (pi.PropertyType.IsGenericType && pi.PropertyType.GetGenericTypeDefinition() == typeof(ICollection<>) && pi.PropertyType.GenericTypeArguments[0].IsMappedTable()))) { return false; }
            return true;
        }
        /// <summary>
        /// Verilen özelliğin (<see cref="PropertyInfo"/>) birincil anahtar (Primary Key) olup olmadığını kontrol eder. Özelliğin, <see cref="KeyAttribute"/> ile işaretlenmiş olup olmadığını kontrol ederek birincil anahtar durumunu döndürür.
        /// </summary>
        /// <param name="pi">Kontrol edilecek özellik.</param>
        /// <returns>Özellik birincil anahtarsa <see langword="true"/>, değilse <see langword="false"/> döner.</returns>
        public static bool IsPK(this PropertyInfo pi) => pi != null && _try.TryCustomAttribute(pi, out KeyAttribute _);
        /// <summary>
        /// Verilen <see cref="PropertyInfo"/> nesnesinin <see cref="NotMappedAttribute"/> ile işaretlenip işaretlenmediğini kontrol eder.
        /// </summary>
        /// <param name="pi">Kontrol edilecek özellik (<see cref="PropertyInfo"/> nesnesi).</param>
        /// <returns>Özellik <see cref="NotMappedAttribute"/> ile işaretlenmişse <see langword="true"/>, aksi takdirde <see langword="false"/> döner.</returns>
        /// <remarks>Bu metot, Entity Framework veya benzeri ORM yapılarında, bir özelliğin veritabanına eşlenip eşlenmediğini hızlıca kontrol etmek için kullanılabilir.</remarks>
        public static bool IsNotMapped(this PropertyInfo pi) => pi != null && _try.TryCustomAttribute(pi, out NotMappedAttribute _);
        /// <summary>
        /// Verilen özelliğin (<see cref="PropertyInfo"/>) veritabanındaki sütun adını döndürür. Özellik <see cref="ColumnAttribute"/> ile işaretlenmişse, bu özniteliğin belirttiği sütun adını; aksi takdirde özelliğin adını döndürür.
        /// </summary>
        /// <param name="pi">Sütun adı alınacak özellik.</param>
        /// <returns>Özelliğin veritabanındaki sütun adı veya özellik adı.</returns>
        public static string GetColumnName(this PropertyInfo pi)
        {
            Guard.CheckNull(pi, nameof(pi));
            return _try.TryCustomAttribute(pi, out ColumnAttribute _ca) ? _ca.Name : pi.Name;
        }
        /// <summary>
        /// Verilen özelliğin (<see cref="PropertyInfo"/>) veritabanında nasıl oluşturulduğunu belirten <see cref="DatabaseGeneratedOption"/> değerini döndürür. Özellik <see cref="DatabaseGeneratedAttribute"/> ile işaretlenmişse, bu özniteliğin belirttiği seçeneği; aksi takdirde null döner.
        /// </summary>
        /// <param name="pi">Kontrol edilecek özellik.</param>
        /// <returns>DatabaseGeneratedOption değeri veya null.</returns>
        public static DatabaseGeneratedOption? GetDatabaseGeneratedOption(this PropertyInfo pi)
        {
            Guard.CheckNull(pi, nameof(pi));
            return (_try.TryCustomAttribute(pi, out DatabaseGeneratedAttribute _dga) ? _dga.DatabaseGeneratedOption : null);
        }
        /// <summary>
        /// Verilen <see cref="PropertyInfo"/> nesnesinin string uzunluğunu belirleyen attribute&#39;lerden <see cref="StringLengthAttribute"/> veya <see cref="MaxLengthAttribute"/> var ise maksimum uzunluğunu döndürür. Attribute yoksa veya property null ise 0 döner.
        /// </summary>
        /// <param name="pi">Uzunluğu alınacak property&#39;si temsil eden <see cref="PropertyInfo"/> nesnesi.</param>
        /// <returns>String property&#39;sinin maksimum uzunluğu, yoksa 0.</returns>
        public static int GetStringOrMaxLength(this PropertyInfo pi)
        {
            if (pi != null)
            {
                if (_try.TryCustomAttribute(pi, out StringLengthAttribute _s)) { return _s.MaximumLength; }
                if (_try.TryCustomAttribute(pi, out MaxLengthAttribute _m)) { return _m.Length; }
            }
            return 0;
        }
    }
}