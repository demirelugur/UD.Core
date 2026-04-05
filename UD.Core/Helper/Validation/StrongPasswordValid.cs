namespace UD.Core.Helper.Validation
{
    using UD.Core.Extensions;
    using UD.Core.Helper;
    using static UD.Core.Helper.GlobalConstants;
    public sealed class StrongPasswordValid
    {
        public static readonly StrongPasswordValid Default = new(8, 16, true, true, true);
        public int minimumLength { get; }
        public int? maximumLength { get; }
        public bool isConsecutive { get; }
        public bool isEmpty { get; }
        public bool isTurkishSpecialCharacter { get; }
        public StrongPasswordValid(int minimumLength, int? maximumLength, bool isConsecutive, bool isEmpty, bool isTurkishSpecialCharacter)
        {
            this.minimumLength = minimumLength;
            this.maximumLength = maximumLength.NullOrDefault();
            this.isConsecutive = isConsecutive;
            this.isEmpty = isEmpty;
            this.isTurkishSpecialCharacter = isTurkishSpecialCharacter;
        }
        public bool TryIsWarning(string value, string name, string surname, out string[] errors)
        {
            Guard.ThrowIfEmpty(value, nameof(value));
            if (this.maximumLength.HasValue) { Guard.ThrowIfZeroOrNegative(this.maximumLength.Value, nameof(this.maximumLength)); }
            var r = new List<string>();
            if (!PasswordGenerator.IsStrongPassword(value, this.minimumLength))
            {
                if (Checks.IsEnglishCurrentUICulture) { r.Add($"The password must have a minimum of {this.minimumLength} characters and contain at least 1 Uppercase Letter, 1 Lowercase Letter, 1 Number and 1 Punctuation mark!"); }
                else { r.Add($"Şifre minimum {this.minimumLength} karakter ve içerisinde en az 1 Büyük Harf, 1 Küçük Harf, 1 Rakam ve 1 Noktalama işareti olmalıdır!"); }
            }
            if (this.maximumLength.HasValue && value.Length > this.maximumLength.Value)
            {
                if (Checks.IsEnglishCurrentUICulture) { r.Add($"Password can be maximum {this.maximumLength.Value} characters!"); }
                else { r.Add($"Şifre maksimum {this.maximumLength.Value} karakter olabilir!"); }
            }
            if (this.isConsecutive && this.checkConsecutive(value))
            {
                if (Checks.IsEnglishCurrentUICulture) { r.Add("The password must not contain 3 consecutive numbers! (123, 987 etc...)"); }
                else { r.Add("Şifre içerisinde 3 ardışık sayı (123, 987 vb...) bulunmamalıdır!"); }
            }
            if (this.isEmpty && value.Contains(' '))
            {
                if (Checks.IsEnglishCurrentUICulture) { r.Add("There should be no empty characters in the password!"); }
                else { r.Add("Şifre içerisinde boş karakter bulunmamalıdır!"); }
            }
            if (this.isTurkishSpecialCharacter && value.Any(ArrayConstants.TurkishSpecialCharacters.Contains))
            {
                var t = String.Join(", ", ArrayConstants.TurkishSpecialCharacters);
                if (Checks.IsEnglishCurrentUICulture) { r.Add($"The password must not contain any letters specific to the Turkish language! ({t})"); }
                else { r.Add($"Şifre içerisinde Türk diline özgü harf ({t}) bulunmamalıdır!"); }
            }
            var passwordSeo = value.ToSeoFriendly();
            if (this.checkNameSurname(passwordSeo, name))
            {
                if (Checks.IsEnglishCurrentUICulture) { r.Add("Your name(s) must not appear in the password!"); }
                else { r.Add("Şifre içerisinde adınız/adlarınız geçmemelidir!"); }
            }
            if (this.checkNameSurname(passwordSeo, surname))
            {
                if (Checks.IsEnglishCurrentUICulture) { r.Add("Your surname(s) must not appear in the password!"); }
                else { r.Add("Şifre içerisinde soyadınız/soyadlarınız geçmemelidir!"); }
            }
            errors = r.ToArray();
            return r.Count > 0;
        }
        private bool checkConsecutive(string password)
        {
            if (password.Length > 2)
            {
                int i, _sayi1, _sayi2, _sayi3, _l = password.Length - 2;
                for (i = 0; i < _l; i++)
                {
                    if (Char.IsDigit(password[i]) && Char.IsDigit(password[i + 1]) && Char.IsDigit(password[i + 2]))
                    {
                        _sayi1 = (password[i] - '0');
                        _sayi2 = (password[i + 1] - '0');
                        _sayi3 = (password[i + 2] - '0');
                        if ((_sayi2 == (_sayi1 + 1) && _sayi3 == (_sayi2 + 1)) || (_sayi2 == (_sayi1 - 1) && _sayi3 == (_sayi2 - 1))) { return true; }
                    }
                }
            }
            return false;
        }
        private bool checkNameSurname(string passwordSeo, string value)
        {
            var values = value.ToStringOrEmpty().ToEnumerable().Select(x => (x == "" ? [] : x.Split(' ').Select(y => y.ToSeoFriendly()).Where(y => y != "").ToArray())).FirstOrDefault();
            if (values.Length > 0) { foreach (var item in values) { if (passwordSeo.Contains(item)) { return true; } } }
            return false;
        }
    }
}