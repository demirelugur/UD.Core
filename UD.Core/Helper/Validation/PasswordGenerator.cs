namespace UD.Core.Helper.Validation
{
    using System.Text;
    using System.Text.RegularExpressions;
    using UD.Core.Extensions;

    public sealed class PasswordGenerator
    {
        public static readonly PasswordGenerator Default = new("ABCDEFGHIJKLMNOPQRSTUVWXYZ", "abcdefghijklmnopqrstuvwxyz", "0123456789", "!@#$%^*()_+[]{}|;:,.?");
        private string uppercasechars { get; }
        private string lowercasechars { get; }
        private string digits { get; }
        private string punctuations { get; }
        private string allchars => String.Join("", this.uppercasechars, this.lowercasechars, this.digits, this.punctuations);
        public PasswordGenerator(string uppercasechars, string lowercasechars, string digits, string punctuations)
        {
            this.uppercasechars = uppercasechars;
            this.lowercasechars = lowercasechars;
            this.digits = digits;
            this.punctuations = punctuations;
        }
        public string Generate()
        {
            Guard.CheckEmpty(this.uppercasechars, nameof(this.uppercasechars));
            Guard.CheckEmpty(this.lowercasechars, nameof(this.lowercasechars));
            Guard.CheckEmpty(this.digits, nameof(this.digits));
            Guard.CheckEmpty(this.punctuations, nameof(this.punctuations));
            int i, _minlength = 4, _maxlength = Random.Shared.Next(_minlength * 2, (_minlength * 4) + 1);
            var sb = new StringBuilder();
            if (_maxlength % _minlength == 0) { this.set_private(sb, _maxlength / _minlength); }
            else
            {
                this.set_private(sb, 1);
                for (i = _minlength; i < _maxlength; i++) { sb.Append(this.allchars[Random.Shared.Next(this.allchars.Length)]); }
            }
            return new(sb.ToString().ToCharArray().Shuffle().ToArray());
        }
        private void set_private(StringBuilder sb, int count)
        {
            int i;
            foreach (var item in new string[] { this.uppercasechars, this.lowercasechars, this.digits, this.punctuations }) { for (i = 0; i < count; i++) { sb.Append(item[Random.Shared.Next(item.Length)]); } }
        }
        public static string GenerateRandomChars(int length, string element = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789")
        {
            Guard.CheckZeroOrNegative(length, nameof(length));
            Guard.CheckEmpty(element, nameof(element));
            return new(Enumerable.Repeat(element, length).Select(x => x[Random.Shared.Next(x.Length)]).ToArray());
        }
        public static bool IsStrongPassword(string value, int minimumlength = 8)
        {
            value = value.ToStringOrEmpty();
            var r = value.Length >= minimumlength;
            if (r) { r = Regex.IsMatch(value, @"[\d]"); }
            if (r) { r = Regex.IsMatch(value, @"[a-z]"); }
            if (r) { r = Regex.IsMatch(value, @"[A-Z]"); }
            if (r) { r = Regex.IsMatch(value, @"[!@#$%^&*()_+\-=\[\]{}|;:',.<>?]"); }
            return r;
        }
    }
}