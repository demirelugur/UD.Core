namespace UD.Core.Helper.Configuration
{
    using System.Text.RegularExpressions;
    using UD.Core.Extensions;
    public sealed class TitleCaseConfiguration
    {
        public enum ConjunctionDilTypes : byte
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
        private ConjunctionDilTypes[] dils = [ConjunctionDilTypes.tr];
        private char[] punctuations = ['(', '-', '/', ':', '.'];
        private string[] upperkeys = [];
        public TitleCaseConfiguration() { }
        public TitleCaseConfiguration WithDils(params ConjunctionDilTypes[] dils)
        {
            this.dils = (dils ?? []).Distinct().ToArray();
            return this;
        }
        public TitleCaseConfiguration WithPunctuations(params char[] punctuations)
        {
            this.punctuations = (punctuations ?? []).Distinct().ToArray();
            return this;
        }
        public TitleCaseConfiguration WithUpperKeys(params string[] upperkeys)
        {
            this.upperkeys = (upperkeys ?? []).Distinct().ToArray();
            return this;
        }
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