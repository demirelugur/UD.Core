namespace UD.Core.Generates
{
    using System.Text;
    using UD.Core.Extensions;
    public sealed class PasswordGenerator
    {
        private readonly string _upperCases;
        private readonly string _lowerCases;
        private readonly string _digits;
        private readonly string _punctuations;
        public PasswordGenerator() : this("", "", "", "") { }
        public PasswordGenerator(string upperCases, string lowerCases, string digits, string punctuations)
        {
            this._upperCases = upperCases.CoalesceOrDefault("ABCDEFGHIJKLMNOPQRSTUVWXYZ");
            this._lowerCases = lowerCases.CoalesceOrDefault("abcdefghijklmnopqrstuvwxyz");
            this._digits = digits.CoalesceOrDefault("0123456789");
            this._punctuations = punctuations.CoalesceOrDefault("!@#$%^*()_+[]{}|;:,.?");
        }
        public string Generate()
        {
            int i, minLength = 4, maxLength = Random.Shared.Next(minLength * 2, (minLength * 4) + 1);
            var sb = new StringBuilder();
            if (maxLength % minLength == 0) { this.Set(sb, maxLength / minLength); }
            else
            {
                this.Set(sb, 1);
                var allowedCharacters = String.Join("", this._upperCases, this._lowerCases, this._digits, this._punctuations).Trim();
                for (i = minLength; i < maxLength; i++) { sb.Append(allowedCharacters[Random.Shared.Next(allowedCharacters.Length)]); }
            }
            return new(sb.ToString().ToCharArray().Shuffle().ToArray());
        }
        private void Set(StringBuilder sb, int count)
        {
            int i;
            foreach (var item in new string[] { this._upperCases, this._lowerCases, this._digits, this._punctuations }) { for (i = 0; i < count; i++) { sb.Append(item[Random.Shared.Next(item.Length)]); } }
        }
    }
}