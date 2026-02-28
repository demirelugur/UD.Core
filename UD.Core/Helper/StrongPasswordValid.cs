namespace UD.Core.Helper
{
    using UD.Core.Extensions;
    public sealed class StrongPasswordValid
    {
        public static readonly StrongPasswordValid Default = new(8, 16, true, true, true);
        public int minimumlength { get; }
        public int? maximumlength { get; }
        public bool isardisiksayi { get; }
        public bool isbosluk { get; }
        public bool isturkceharf { get; }
        public string ad { get; }
        public string soyad { get; }
        public StrongPasswordValid(int minimumlength, int? maximumlength, bool isardisiksayi, bool isbosluk, bool isturkceharf)
        {
            this.minimumlength = minimumlength;
            this.maximumlength = maximumlength.NullOrDefault();
            this.isardisiksayi = isardisiksayi;
            this.isbosluk = isbosluk;
            this.isturkceharf = isturkceharf;
        }
        public bool TryIsWarning(string value, string ad, string soyad, string dil, out string[] errors)
        {
            Guard.CheckEmpty(value, nameof(value));
            Guard.UnSupportLanguage(dil, nameof(dil));
            if (this.maximumlength.HasValue) { Guard.CheckZeroOrNegative(this.maximumlength.Value, nameof(this.maximumlength)); }
            var _r = new List<string>();
            var _isen = dil == "en";
            if (!PasswordGenerator.IsStrongPassword(value, this.minimumlength))
            {
                if (_isen) { _r.Add($"The password must have a minimum of {this.minimumlength.ToString()} characters and contain at least 1 Uppercase Letter, 1 Lowercase Letter, 1 Number and 1 Punctuation mark!"); }
                else { _r.Add($"Şifre minimum {this.minimumlength.ToString()} karakter ve içerisinde en az 1 Büyük Harf, 1 Küçük Harf, 1 Rakam ve 1 Noktalama işareti olmalıdır!"); }
            }
            if (this.maximumlength.HasValue && value.Length > this.maximumlength.Value)
            {
                if (_isen) { _r.Add($"Password can be maximum {this.maximumlength.Value.ToString()} characters!"); }
                else { _r.Add($"Şifre maksimum {this.maximumlength.Value.ToString()} karakter olabilir!"); }
            }
            if (this.isardisiksayi && this.ardisiksayikontrol_private(value))
            {
                if (_isen) { _r.Add("The password must not contain 3 consecutive numbers! (123, 987 etc...)"); }
                else { _r.Add("Şifre içerisinde 3 ardışık sayı (123, 987 vb...) bulunmamalıdır!"); }
            }
            if (this.isbosluk && value.Contains(' '))
            {
                if (_isen) { _r.Add("There should be no empty characters in the password!"); }
                else { _r.Add("Şifre içerisinde boş karakter bulunmamalıdır!"); }
            }
            if (this.isturkceharf && value.Any(GlobalConstants.turkishcharacters.Contains))
            {
                var _t = String.Join(", ", GlobalConstants.turkishcharacters);
                if (_isen) { _r.Add($"The password must not contain any letters specific to the Turkish language! ({_t})"); }
                else { _r.Add($"Şifre içerisinde Türk diline özgü harf ({_t}) bulunmamalıdır!"); }
            }
            var _password_seo = value.ToSeoFriendly();
            if (this.adsoyadkontrol_private(_password_seo, ad))
            {
                if (_isen) { _r.Add("Your name(s) must not appear in the password!"); }
                else { _r.Add("Şifre içerisinde adınız/adlarınız geçmemelidir!"); }
            }
            if (this.adsoyadkontrol_private(_password_seo, soyad))
            {
                if (_isen) { _r.Add("Your surname(s) must not appear in the password!"); }
                else { _r.Add("Şifre içerisinde soyadınız/soyadlarınız geçmemelidir!"); }
            }
            errors = _r.ToArray();
            return _r.Count > 0;
        }
        private bool ardisiksayikontrol_private(string password)
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
        private bool adsoyadkontrol_private(string passwordseo, string value)
        {
            var _values = value.ToStringOrEmpty().ToEnumerable().Select(x => (x == "" ? Array.Empty<string>() : x.Split(' ').Select(y => y.ToSeoFriendly()).Where(y => y != "").ToArray())).FirstOrDefault();
            if (_values.Length > 0) { foreach (var item in _values) { if (passwordseo.Contains(item)) { return true; } } }
            return false;
        }
    }
}