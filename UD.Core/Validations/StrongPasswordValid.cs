namespace UD.Core.Validations
{
    using UD.Core.Extensions;
    using UD.Core.Helper;
    using static UD.Core.Helper.GlobalConstants;
    public sealed class StrongPasswordValid
    {
        private readonly int _minimumLength;
        private readonly int? _maximumLength;
        private readonly bool _isConsecutive;
        private readonly bool _isEmptyCharacter;
        private readonly bool _isTurkishSpecialCharacter;
        public StrongPasswordValid() : this(8, 16, true, true, true) { }
        public StrongPasswordValid(int minimumLength, int? maximumLength, bool isConsecutive, bool isEmptyCharacter, bool isTurkishSpecialCharacter)
        {
            this._minimumLength = minimumLength;
            this._maximumLength = maximumLength.NullOrDefault();
            this._isConsecutive = isConsecutive;
            this._isEmptyCharacter = isEmptyCharacter;
            this._isTurkishSpecialCharacter = isTurkishSpecialCharacter;
        }
        public bool TryIsWarning(string value, string name, string surname, out string[] errors)
        {
            var r = new List<string>();
            if (!Checks.IsStrongPassword(value, this._minimumLength)) { r.Add($"Şifre minimum {this._minimumLength} karakter ve içerisinde en az 1 Büyük Harf, 1 Küçük Harf, 1 Rakam ve 1 Noktalama işareti olmalıdır!"); }
            if (this._maximumLength.HasValue && value.Length > this._maximumLength.Value) { r.Add($"Şifre maksimum {this._maximumLength.Value} karakter olabilir!"); }
            if (this._isConsecutive && CheckConsecutive(value)) { r.Add("Şifre içerisinde 3 ardışık sayı (123, 987 vb...) bulunmamalıdır!"); }
            if (this._isEmptyCharacter && value.Contains(' ')) { r.Add("Şifre içerisinde boş karakter bulunmamalıdır!"); }
            if (this._isTurkishSpecialCharacter && value.Any(ArrayConstants.TurkishSpecialCharacters.Contains)) { r.Add($"Şifre içerisinde Türk diline özgü harf ({String.Join(", ", ArrayConstants.TurkishSpecialCharacters)}) bulunmamalıdır!"); }
            var valueSeo = value.ToSeoFriendly();
            if (CheckFullName(valueSeo, name)) { r.Add("Şifre içerisinde adınız / adlarınız geçmemelidir!"); }
            if (CheckFullName(valueSeo, surname)) { r.Add("Şifre içerisinde soyadınız / soyadlarınız geçmemelidir!"); }
            errors = r.ToArray();
            return r.Count > 0;
        }
        private static bool CheckConsecutive(string password)
        {
            if (password.Length > 2)
            {
                int i, no1, no2, no3, l = password.Length - 2;
                for (i = 0; i < l; i++)
                {
                    if (Char.IsDigit(password[i]) && Char.IsDigit(password[i + 1]) && Char.IsDigit(password[i + 2]))
                    {
                        no1 = (password[i] - '0');
                        no2 = (password[i + 1] - '0');
                        no3 = (password[i + 2] - '0');
                        if ((no2 == (no1 + 1) && no3 == (no2 + 1)) || (no2 == (no1 - 1) && no3 == (no2 - 1))) { return true; }
                    }
                }
            }
            return false;
        }
        private static bool CheckFullName(string valueSeo, string value)
        {
            var values = value.ToStringOrEmpty().ToEnumerable().Select(x => (x == "" ? [] : x.Split(' ').Select(y => y.ToSeoFriendly()).Where(y => y != "").ToArray())).FirstOrDefault();
            if (values.Length > 0) { foreach (var item in values) { if (valueSeo.Contains(item)) { return true; } } }
            return false;
        }
    }
}