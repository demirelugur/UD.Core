namespace UD.Core.Helper.Generates
{
    using System.Text;
    using UD.Core.Helper.Validations;
    public sealed class PasswordGenerator
    {
        public static readonly PasswordGenerator Default = new("ABCDEFGHIJKLMNOPQRSTUVWXYZ", "abcdefghijklmnopqrstuvwxyz", "0123456789", "!@#$%^*()_+[]{}|;:,.?");
        private string upperCases { get; }
        private string lowerCases { get; }
        private string digits { get; }
        private string punctuations { get; }
        private string all => String.Join("", this.upperCases, this.lowerCases, this.digits, this.punctuations);
        public PasswordGenerator(string upperCases, string lowerCases, string digits, string punctuations)
        {
            this.upperCases = upperCases;
            this.lowerCases = lowerCases;
            this.digits = digits;
            this.punctuations = punctuations;
        }
        public string Generate()
        {
            Guard.ThrowIfEmpty(this.upperCases, nameof(this.upperCases));
            Guard.ThrowIfEmpty(this.lowerCases, nameof(this.lowerCases));
            Guard.ThrowIfEmpty(this.digits, nameof(this.digits));
            Guard.ThrowIfEmpty(this.punctuations, nameof(this.punctuations));
            int i, minLength = 4, maxLength = Random.Shared.Next(minLength * 2, (minLength * 4) + 1);
            var sb = new StringBuilder();
            if (maxLength % minLength == 0) { this.set(sb, maxLength / minLength); }
            else
            {
                this.set(sb, 1);
                for (i = minLength; i < maxLength; i++) { sb.Append(this.all[Random.Shared.Next(this.all.Length)]); }
            }
            return new(sb.ToString().ToCharArray().Shuffle().ToArray());
        }
        private void set(StringBuilder sb, int count)
        {
            int i;
            foreach (var item in new string[] { this.upperCases, this.lowerCases, this.digits, this.punctuations }) { for (i = 0; i < count; i++) { sb.Append(item[Random.Shared.Next(item.Length)]); } }
        }
    }
}