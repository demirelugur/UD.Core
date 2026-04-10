namespace UD.Core.Helper.Configuration
{
    using System.Globalization;
    using System.Text.RegularExpressions;
    using UD.Core.Extensions;
    public sealed class TitleCaseConfiguration
    {
        public enum EnumConjunctionLanguage : byte
        {
            /// <summary>Küçük harfe çevirilecek bağlaçlar: <c>Ancak,Ama,Da,De,Fakat,Gibi,İle,İse,Ki,Lakin,Ve,Veya</c></summary>
            tr = 1,
            /// <summary>Küçük harfe çevirilecek bağlaçlar: <c>A,An,And,As,By,For,In,İn,Of,On,Or,The,To,With</c></summary>
            en
        }
        private EnumConjunctionLanguage[] languages = [EnumConjunctionLanguage.tr];
        private char[] punctuations = ['(', '-', '/', ':', '.'];
        private string[] upperKeys = [];
        private CultureInfo culture = new("tr-TR");
        public TitleCaseConfiguration() { }
        public TitleCaseConfiguration WithDils(params EnumConjunctionLanguage[] dils)
        {
            this.languages = (dils ?? []).Distinct().ToArray();
            return this;
        }
        public TitleCaseConfiguration WithPunctuations(params char[] punctuations)
        {
            this.punctuations = (punctuations ?? []).Distinct().ToArray();
            return this;
        }
        public TitleCaseConfiguration WithUpperKeys(params string[] upperkeys)
        {
            this.upperKeys = (upperkeys ?? []).Distinct().ToArray();
            return this;
        }
        public TitleCaseConfiguration WithCulture(CultureInfo culture)
        {
            this.culture = culture ?? throw new ArgumentNullException(nameof(culture));
            return this;
        }
        public string Execute(string value)
        {
            if (value.IsNullOrWhiteSpace()) { return ""; }
            var lower = this.culture.TextInfo.ToLower(value.Trim());
            var title = lower.ToTitleCase(true, this.punctuations, this.culture);
            if (this.languages.Length > 0)
            {
                var hashSet = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
                if (this.languages.Contains(EnumConjunctionLanguage.tr)) { hashSet.UnionWith("Ancak,Ama,Da,De,Fakat,Gibi,İle,İse,Ki,Lakin,Ve,Veya".Split(',')); }
                if (this.languages.Contains(EnumConjunctionLanguage.en)) { hashSet.UnionWith("A,An,And,As,By,For,In,İn,Of,On,Or,The,To,With".Split(',')); }
                title = Regex.Replace(title, $@"\b({String.Join("|", hashSet)})\b", m => m.Value.ToLower(this.culture), RegexOptions.IgnoreCase);
            }
            foreach (var item in this.upperKeys) { title = title.Replace(item, item.ToUpper(this.culture)); }
            return title;
        }
    }
}