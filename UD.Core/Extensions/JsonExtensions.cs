namespace UD.Core.Extensions
{
    using Newtonsoft.Json.Linq;
    using System;
    using UD.Core.Helper;
    public static class JsonExtensions
    {
        /// <summary><paramref name="jToken"/> değeri null, <see cref="JTokenType.None"/>, <see cref="JTokenType.Null"/> veya <see cref="JTokenType.Undefined"/> ise <see langword="true"/> döner; aksi takdirde <see langword="false"/> döner. Bu metot, bir <see cref="JToken"/> nesnesinin geçerli bir değere sahip olup olmadığını kontrol etmek için kullanılır.</summary>
        public static bool IsNullOrUndefined(this JToken jToken) => (jToken == null || jToken.Type.Includes(JTokenType.None, JTokenType.Null, JTokenType.Undefined));
        /// <summary>Bir <see cref="JToken"/> nesnesini belirtilen <typeparamref name="TKey"/> türündeki bir diziye dönüştürür. Eğer <see cref="JToken"/> null ise boş bir dizi döner, array türünde ise içindeki değerleri <typeparamref name="TKey"/> türüne çevirip dizi olarak döner. Diğer durumlarda bir istisna fırlatır.</summary>
        /// <typeparam name="TKey">Dönüştürülecek hedef veri türü.</typeparam>
        /// <param name="jToken">Dönüştürülecek <see cref="JToken"/> nesnesi.</param>
        /// <returns><typeparamref name="TKey"/> türünden bir dizi.</returns>
        /// <exception cref="NotSupportedException"><see cref="JToken"/> türü null veya array değilse fırlatılır.</exception>
        public static TKey[] ToArrayFromJToken<TKey>(this JToken jToken)
        {
            if (jToken.IsNullOrUndefined()) { return []; }
            if (jToken.Type == JTokenType.Array) { return jToken.Select(x => x.Value<TKey>()).ToArray(); }
            if (Checks.IsEnglishCurrentUICulture) { throw new NotSupportedException($"The type of \"{nameof(jToken)}\" is incompatible!"); }
            throw new NotSupportedException($"\"{nameof(jToken)}\" türü uyumsuzdur!");
        }
    }
}