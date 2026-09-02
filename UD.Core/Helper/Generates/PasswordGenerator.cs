namespace UD.Core.Helper.Generates
{
    using System.Text;
    using UD.Core.Extensions;
    public sealed class PasswordGenerator
    {
        private readonly string _upperCases;
        private readonly string _lowerCases;
        private readonly string _digits;
        private readonly string _punctuations;
        private readonly string _allowedCharacters;
        public PasswordGenerator() : this("", "", "", "") { }
        public PasswordGenerator(string upperCases, string lowerCases, string digits, string punctuations)
        {
            this._upperCases = upperCases.CoalesceOrDefault("ABCDEFGHIJKLMNOPQRSTUVWXYZ");
            this._lowerCases = lowerCases.CoalesceOrDefault("abcdefghijklmnopqrstuvwxyz");
            this._digits = digits.CoalesceOrDefault("0123456789");
            this._punctuations = punctuations.CoalesceOrDefault("!@#$%^*()_+[]{}|;:,.?");
            this._allowedCharacters = String.Join("", this._upperCases, this._lowerCases, this._digits, this._punctuations).Trim();
        }
        public string Generate()
        {
            int i, minLength = 4, maxLength = Random.Shared.Next(minLength * 2, (minLength * 4) + 1);
            var sb = new StringBuilder();
            if (maxLength % minLength == 0) { this.Set(sb, maxLength / minLength); }
            else
            {
                this.Set(sb, 1);
                for (i = minLength; i < maxLength; i++) { sb.Append(this._allowedCharacters[Random.Shared.Next(this._allowedCharacters.Length)]); }
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