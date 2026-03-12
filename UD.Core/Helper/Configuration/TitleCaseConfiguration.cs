namespace UD.Core.Helper.Configuration
{
    using System.Text.RegularExpressions;
    using UD.Core.Extensions;

    /// <summary>
    /// Metinleri başlık formatına (title case) dönüştürmek için yapılandırma sağlayan bir sınıftır. Türkçe ve İngilizce gibi diller için özel kurallar ve özelleştirilebilir ayarlar sunar.
    /// </summary>
    public sealed class TitleCaseConfiguration
    {
        /// <summary>
        /// Desteklenen dil türlerini temsil eden bir enum yapısıdır.
        /// </summary>
        public enum ConjunctionDilTypes
        {
            /// <summary>
            /// Küçük harfe çevirilecek bağlaçlar: <c>Ancak,Ama,Da,De,Fakat,Gibi,İle,İse,Ki,Lakin,Ve,Veya</c>
            /// </summary>
            tr = 1,
            /// <summary>
            /// Küçük harfe çevirilecek bağlaçlar: <c>A,An,And,As,By,For,In,İn,Of,On,Or,The,To,With</c>
            /// </summary>
            en
        }
        /// <summary>
        /// Dönüştürme işlemi için kullanılacak dillerin listesidir.
        /// Varsayılan olarak yalnızca Türkçe (tr) içerir.
        /// </summary>
        private ConjunctionDilTypes[] dils = [ConjunctionDilTypes.tr];
        /// <summary>
        /// Başlık formatında kelime ayırıcı olarak kullanılacak noktalama işaretlerinin listesidir. Varsayılan olarak &#39;(&#39;, &#39;-&#39;, &#39;/&#39;, &#39;:&#39;, &#39;.&#39; içerir.
        /// </summary>
        private char[] punctuations = ['(', '-', '/', ':', '.'];
        /// <summary>
        /// Her zaman büyük harfle yazılması gereken kelimelerin listesidir.
        /// Varsayılan olarak boştur.
        /// </summary>
        private string[] upperkeys = [];
        /// <summary>
        /// Varsayılan yapılandırıcı. Başlangıç değerleriyle bir TitleCaseConfiguration nesnesi oluşturur.
        /// </summary>
        public TitleCaseConfiguration() { }
        /// <summary>
        /// Dönüştürme işlemi için kullanılacak dilleri ayarlar.
        /// </summary>
        /// <param name="dils">Kullanılacak dil türlerinin listesidir. Null ise boş bir dizi kullanılır.</param>
        public TitleCaseConfiguration WithDils(params ConjunctionDilTypes[] dils)
        {
            this.dils = (dils ?? []).Distinct().ToArray();
            return this;
        }
        /// <summary>
        /// Başlık formatında kelime ayırıcı olarak kullanılacak noktalama işaretlerini ayarlar.
        /// </summary>
        /// <param name="punctuations">Kullanılacak noktalama işaretlerinin listesidir. Null ise boş bir dizi kullanılır.</param>
        public TitleCaseConfiguration WithPunctuations(params char[] punctuations)
        {
            this.punctuations = (punctuations ?? []).Distinct().ToArray();
            return this;
        }
        /// <summary>
        /// Her zaman büyük harfle yazılması gereken kelimeleri ayarlar.
        /// </summary>
        /// <param name="upperkeys">Büyük harfle yazılacak kelimelerin listesidir. Null ise boş bir dizi kullanılır.</param>
        public TitleCaseConfiguration WithUpperKeys(params string[] upperkeys)
        {
            this.upperkeys = (upperkeys ?? []).Distinct().ToArray();
            return this;
        }
        /// <summary>
        /// Verilen metni, yapılandırmaya uygun şekilde başlık formatına dönüştürür. Türkçe ve İngilizce için özel küçük harf kuralları uygular ve belirtilen kelimeleri büyük harfe çevirir.
        /// </summary>
        /// <param name="value">Dönüştürülecek metin.</param>
        /// <returns>Başlık formatına dönüştürülmüş metni döndürür. Boş metin için boş string döner.</returns>
        public string Execute(string value)
        {
            value = value.ReplaceTRNSpace().RemoveMultipleSpace();
            if (value == "") { return ""; }
            value = value.ToTitleCase(true, this.punctuations);
            if (this.dils.Length > 0)
            {
                var l = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
                if (this.dils.Contains(ConjunctionDilTypes.tr)) { l.UnionWith("Ancak,Ama,Da,De,Fakat,Gibi,İle,İse,Ki,Lakin,Ve,Veya".Split(',')); }
                if (this.dils.Contains(ConjunctionDilTypes.en)) { l.UnionWith("A,An,And,As,By,For,In,İn,Of,On,Or,The,To,With".Split(',')); }
                value = Regex.Replace(value, $@"\b({String.Join("|", l)})\b", x => x.Value.ToLower(), RegexOptions.IgnoreCase);
            }
            foreach (var item in this.upperkeys) { value = value.Replace(item, item.ToUpper()); }
            return value;
        }
    }
}