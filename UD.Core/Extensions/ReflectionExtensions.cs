using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection;
using UD.Core.Attributes; // <see langword="true"/>
using UD.Core.Helper;
namespace UD.Core.Extensions
{
    public static class ReflectionExtensions
    {
        #region PropertyInfo
        /// <summary><paramref name="propertyInfo"/> özelliğinin veritabanına eşlenip eşlenmediğini kontrol eder. Özellik, <see cref="NotMappedAttribute"/> ile işaretlenmişse, bu metot <see langword="false"/> döner; aksi takdirde <see langword="true"/> döner.</summary>
        /// <param name="propertyInfo">Kontrol edilecek özellik bilgisi.</param>
        /// <returns>Özellik veritabanına eşlenmişse <see langword="true"/>, aksi takdirde <see langword="false"/> döner.</returns>
        public static bool IsMapped(this PropertyInfo propertyInfo)
        {
            ArgumentNullException.ThrowIfNull(propertyInfo, nameof(propertyInfo));
            if (!propertyInfo.CanRead || !propertyInfo.CanWrite || propertyInfo.IsNotMappedAttribute()) { return false; }
            var getMethod = propertyInfo.GetMethod;
            if (getMethod == null) { return false; }
            if (getMethod.IsVirtual && !getMethod.IsFinal) { return false; }
            return true;
        }
        /// <summary>Verilen özelliğin (<see cref="PropertyInfo"/>) birincil anahtar (Primary Key) olup olmadığını kontrol eder. Özelliğin, <see cref="KeyAttribute"/> ile işaretlenmiş olup olmadığını kontrol ederek birincil anahtar durumunu döndürür.</summary>
        /// <param name="propertyInfo">Kontrol edilecek özellik.</param>
        /// <returns>Özellik birincil anahtarsa <see langword="true"/>, değilse <see langword="false"/> döner.</returns>
        public static bool IsPK(this PropertyInfo propertyInfo) => TryValidators.TryCustomAttribute(propertyInfo, out KeyAttribute _);
        /// <summary>Verilen <see cref="PropertyInfo"/> nesnesinin <see cref="NotMappedAttribute"/> ile işaretlenip işaretlenmediğini kontrol eder.</summary>
        /// <param name="propertyInfo">Kontrol edilecek özellik (<see cref="PropertyInfo"/> nesnesi).</param>
        /// <returns>Özellik <see cref="NotMappedAttribute"/> ile işaretlenmişse <see langword="true"/>, aksi takdirde <see langword="false"/> döner.</returns>
        /// <remarks>Bu metot, Entity Framework veya benzeri ORM yapılarında, bir özelliğin veritabanına eşlenip eşlenmediğini hızlıca kontrol etmek için kullanılabilir.</remarks>
        public static bool IsNotMappedAttribute(this PropertyInfo propertyInfo) => TryValidators.TryCustomAttribute(propertyInfo, out NotMappedAttribute _);
        /// <summary>Verilen özelliğin (<see cref="PropertyInfo"/>) HTML içeriği içerip içermediğini kontrol eder. Özellik, <see cref="HtmlContentAttribute"/> ile işaretlenmişse, bu metot <see langword="true"/> döner; aksi takdirde <see langword="false"/> döner.</summary>
        public static bool IsHtmlContent(this PropertyInfo propertyInfo) => TryValidators.TryCustomAttribute(propertyInfo, out HtmlContentAttribute _);
        /// <summary>Verilen özelliğin (<see cref="PropertyInfo"/>) temizlenmeden (sanitize) geçirilmesi gerektiğini belirten <see cref="SkipSanitizeAttribute"/> ile işaretlenip işaretlenmediğini kontrol eder. Eğer özellik bu özniteliğe sahipse, bu metot <see langword="true"/> döner; aksi takdirde <see langword="false"/> döner.</summary>
        public static bool IsSkipSanitize(this PropertyInfo propertyInfo) => TryValidators.TryCustomAttribute(propertyInfo, out SkipSanitizeAttribute _);
        /// <summary>Verilen özelliğin (<see cref="PropertyInfo"/>) zorunlu (required) olup olmadığını kontrol eder. Özellik, <see cref="RequiredAttribute"/> ile işaretlenmişse, bu metot <see langword="true"/> döner; aksi takdirde <see langword="false"/> döner.</summary>
        public static bool IsRequired(this PropertyInfo propertyInfo) => TryValidators.TryCustomAttribute(propertyInfo, out RequiredAttribute _);
        /// <summary>Verilen özelliğin (<see cref="PropertyInfo"/>) veritabanındaki sütun adını döndürür. Özellik <see cref="ColumnAttribute"/> ile işaretlenmişse, bu özniteliğin belirttiği sütun adını; aksi takdirde özelliğin adını döndürür.</summary>
        /// <param name="propertyInfo">Sütun adı alınacak özellik.</param>
        /// <returns>Özelliğin veritabanındaki sütun adı veya özellik adı.</returns>
        public static string GetColumnName(this PropertyInfo propertyInfo) => (TryValidators.TryCustomAttribute(propertyInfo, out ColumnAttribute _ca) && !_ca.Name.IsNullOrEmpty()) ? _ca.Name : propertyInfo.Name;
        /// <summary>Verilen özelliğin (<see cref="PropertyInfo"/>) veritabanında nasıl oluşturulduğunu belirten <see cref="DatabaseGeneratedOption"/> değerini döndürür. Özellik <see cref="DatabaseGeneratedAttribute"/> ile işaretlenmişse, bu özniteliğin belirttiği seçeneği; aksi takdirde null döner.</summary>
        /// <param name="propertyInfo">Kontrol edilecek özellik.</param>
        /// <returns>DatabaseGeneratedOption değeri veya null.</returns>
        public static DatabaseGeneratedOption? GetDatabaseGeneratedOption(this PropertyInfo propertyInfo) => (TryValidators.TryCustomAttribute(propertyInfo, out DatabaseGeneratedAttribute _dga) ? _dga.DatabaseGeneratedOption : null);
        /// <summary>Verilen <see cref="PropertyInfo"/> nesnesinin string uzunluğunu belirleyen attribute&#39;lerden <see cref="StringLengthAttribute"/> veya <see cref="MaxLengthAttribute"/> var ise maksimum uzunluğunu döndürür. Attribute yoksa veya property null ise 0 döner.</summary>
        /// <param name="propertyInfo">Uzunluğu alınacak property&#39;si temsil eden <see cref="PropertyInfo"/> nesnesi.</param>
        /// <returns>String property&#39;sinin maksimum uzunluğu, yoksa 0.</returns>
        public static int GetStringOrMaxLength(this PropertyInfo propertyInfo)
        {
            if (propertyInfo != null)
            {
                if (TryValidators.TryCustomAttribute(propertyInfo, out StringLengthAttribute _sl)) { return _sl.MaximumLength; }
                if (TryValidators.TryCustomAttribute(propertyInfo, out MaxLengthAttribute _ml)) { return _ml.Length; }
            }
            return 0;
        }
        #endregion
        #region MemberInfo
        /// <summary>Verilen <paramref name="memberInfo"/> nesnesinin <see cref="SkipAuditLogAttribute"/> ile işaretlenip işaretlenmediğini kontrol eder. Eğer nesne bu özniteliğe sahipse, bu metot <see langword="true"/> döner; aksi takdirde <see langword="false"/> döner.</summary>
        public static bool IsSkipAuditLog(this MemberInfo memberInfo)
        {
            try
            {
                var attr = memberInfo.GetCustomAttribute<SkipAuditLogAttribute>();
                return attr != null;
            }
            catch { return false; }
        }
        /// <summary>Verilen <see cref="MemberInfo"/> nesnesine tanımlanmış olan <see cref="DescriptionAttribute"/> bilgisini döndürür. Eğer attribute yoksa veya hata oluşursa boş string (&quot;&quot;) döner.</summary>
        public static string GetDescription(this MemberInfo memberInfo)
        {
            try
            {
                var attr = memberInfo.GetCustomAttribute<DescriptionAttribute>();
                return attr == null ? "" : attr.Description.ToStringOrEmpty();
            }
            catch { return ""; }
        }
        /// <summary>Verilen <see cref="MemberInfo"/> nesnesine tanımlanmış olan <see cref="DisplayAttribute"/> bilgisini döndürür. Eğer attribute yoksa veya hata oluşursa boş string (&quot;&quot;) döner.</summary>
        public static string GetDisplayName(this MemberInfo memberInfo)
        {
            try
            {
                var attr = memberInfo.GetCustomAttribute<DisplayAttribute>();
                return attr == null ? "" : attr.GetName().ToStringOrEmpty();
            }
            catch { return ""; }
        }
        #endregion
    }
}