namespace UD.Core.Extensions
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using UD.Core.Helper;
    using UD.Core.Helper.Validation;
    public static class DataAnnotationExtensions
    {
        /// <summary>Verilen validation bağlamında bir özelliğin gerekli olup olmadığını kontrol eder.</summary>
        /// <param name="validationContext">ValidationContext nesnesi.</param>
        /// <returns>Gerekli ise <see langword="true"/>, değilse <see langword="false"/> döner.</returns>
        public static bool IsRequiredAttribute(this ValidationContext validationContext)
        {
            Guard.ThrowIfNull(validationContext, nameof(validationContext));
            var property = validationContext.ObjectInstance.GetType().GetProperty(validationContext.MemberName);
            return (property != null && TryValidators.TryCustomAttribute(property, out RequiredAttribute _));
        }
        /// <summary>Verilen doğrulama bağlamına göre, belirtilen özelliğin değerini günceller. Eğer özellik yazılabilir durumdaysa, yeni değer atanır.</summary>
        /// <param name="validationContext">Doğrulama işlemi sırasında bağlam bilgilerini içeren nesne.</param>
        /// <param name="value">Güncellenmek istenen yeni değer.</param>
        /// <exception cref="ArgumentNullException">Eğer <paramref name="validationContext"/> null ise tetiklenir.</exception>
        public static void SetValidatePropertyValue(this ValidationContext validationContext, object value)
        {
            Guard.ThrowIfNull(validationContext, nameof(validationContext));
            var property = validationContext.ObjectType.GetProperty(validationContext.MemberName);
            if (property != null && property.CanWrite) { property.SetValue(validationContext.ObjectInstance, value); }
        }
    }
}