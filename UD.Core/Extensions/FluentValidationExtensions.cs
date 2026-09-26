namespace UD.Core.Extensions
{
    using FluentValidation;
    using System;
    public static class FluentValidationExtensions
    {
        private static IRuleBuilderOptions<T, string> MustStringHasValue<T>(this IRuleBuilder<T, string> ruleBuilder) => ruleBuilder.Must(value => !value.IsNullOrEmpty());
        /// <summary><paramref name="ruleBuilder"/> için boş olamaz kuralını uygular ve hata mesajını <paramref name="fieldName"/> ile birlikte döndürür</summary>
        public static IRuleBuilderOptions<T, string> RequiredString<T>(this IRuleBuilder<T, string> ruleBuilder, string fieldName) => ruleBuilder.MustStringHasValue().WithMessage($"{fieldName}, boş olamaz");
        /// <summary><paramref name="ruleBuilder"/> için boş olamaz ve maksimum uzunluk kuralını uygular ve hata mesajını <paramref name="fieldName"/> ile birlikte döndürür</summary>
        public static IRuleBuilderOptions<T, string> RequiredStringWithMaximumLength<T>(this IRuleBuilder<T, string> ruleBuilder, int maxLength, string fieldName) => ruleBuilder.RequiredString(fieldName).ValidMaximumLength(maxLength, fieldName);
        /// <summary><paramref name="ruleBuilder"/> için boş olamaz ve telefon numarası biçiminde olma kuralını uygular ve hata mesajını döndürür</summary>
        public static IRuleBuilderOptions<T, string> RequiredPhone<T>(this IRuleBuilder<T, string> ruleBuilder) => ruleBuilder.MustStringHasValue().WithMessage("Telefon numarasını, \"(5xx) (xxx)-xxxx\" biçiminde belirtiniz");
        /// <summary><paramref name="ruleBuilder"/> için boş olamaz ve Guid.Empty olmama kuralını uygular ve hata mesajını <paramref name="fieldName"/> ile birlikte döndürür</summary>
        public static IRuleBuilderOptions<T, Guid> RequiredGuid<T>(this IRuleBuilder<T, Guid> ruleBuilder, string fieldName) => ruleBuilder.Must(value => value != Guid.Empty).WithMessage($"{fieldName}, boş olamaz");
        /// <summary><paramref name="ruleBuilder"/> için geçerli bir enum değeri olma kuralını uygular ve hata mesajını <paramref name="title"/> ile birlikte döndürür</summary>
        public static IRuleBuilderOptions<T, TEnum> ValidEnumDefined<T, TEnum>(this IRuleBuilder<T, TEnum> ruleBuilder, string title) where TEnum : struct, Enum => ruleBuilder.Must(Enum.IsDefined).WithMessage($"Geçerli bir \"{title}\" tipi belirtiniz");
        /// <summary><paramref name="ruleBuilder"/> için geçerli bir T.C. Kimlik Numarası olma kuralını uygular ve hata mesajını döndürür</summary>
        public static IRuleBuilderOptions<T, long> ValidTCKimlikNo<T>(this IRuleBuilder<T, long> ruleBuilder) => ruleBuilder.Must(x => x.IsTRIdentityNumber()).WithMessage("Geçerli bir T.C. Kimlik Numarası giriniz");
        /// <summary><paramref name="ruleBuilder"/> için geçerli bir T.C. Vergi Kimlik Numarası olma kuralını uygular ve hata mesajını döndürür</summary>
        public static IRuleBuilderOptions<T, long> ValidVergiKimlikNo<T>(this IRuleBuilder<T, long> ruleBuilder) => ruleBuilder.Must(x => x.IsTRTaxIdentityNumber()).WithMessage("Geçerli bir T.C. Vergi Kimlik Numarası giriniz");
        /// <summary><paramref name="ruleBuilder"/> için maksimum uzunluk kuralını uygular ve hata mesajını <paramref name="fieldName"/> ile birlikte döndürür</summary>
        public static IRuleBuilderOptions<T, string> ValidMaximumLength<T>(this IRuleBuilder<T, string> ruleBuilder, int maxLength, string fieldName) => ruleBuilder.MaximumLength(maxLength).WithMessage($"{fieldName}, en fazla \"{maxLength}\" karakter uzunluğunda olabilir");
        /// <summary>
        /// Alanın değerinin ilgili veri tipinin varsayılan değerinden büyük olmasını doğrular.
        /// <para>Sayısal veri tiplerinde varsayılan değer 0 olduğundan, değerin 0&#39;dan büyük olması beklenir.</para>
        /// <para>DateTime, DateTimeOffset ve DateOnly veri tiplerinde ise değerin ilgili tipin varsayılan tarih değerinden büyük olması beklenir ve doğrulama hatası tarih formatına uygun şekilde bildirilir.</para>
        /// </summary>
        public static IRuleBuilderOptions<T, TProperty> GreaterThanDefaultValue<T, TProperty>(this IRuleBuilder<T, TProperty> ruleBuilder, string fieldName) where TProperty : IComparable<TProperty>, IComparable
        {
            var rb = ruleBuilder.GreaterThan(default(TProperty)!);
            if (typeof(TProperty).Includes(typeof(DateTimeOffset), typeof(DateTime), typeof(DateOnly))) { return rb.WithMessage($"{fieldName}, geçerli bir tarih olmalıdır"); }
            return rb.WithMessage($"{fieldName}, 0'dan büyük bir değer olmalıdır");
        }
    }
}