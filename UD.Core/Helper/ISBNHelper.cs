namespace UD.Core.Helper
{
    using System;
    using UD.Core.Extensions;
    using static UD.Core.Helper.GlobalConstants;
    public sealed class ISBNHelper
    {
        public ISBNHelper(string isbn) => this.setisbn(isbn);
        public string Isbn10
        {
            get { return _isbn10; }
            set
            {
                if (TryIsValid(value, out string _c)) { this.setisbn(_c); }
                throw new NotSupportedException($"{_title.isbn} uyumsuzdur!");
            }
        }
        public string Isbn13
        {
            get { return _isbn13; }
            set
            {
                if (TryIsValid(value, out string _c)) { this.setisbn(_c); }
                throw new NotSupportedException($"{_title.isbn} uyumsuzdur!");
            }
        }
        public static string Convert10to13(string isbn)
        {
            string isbn13, isbn10 = cleanisbn(isbn);
            if (isbn10.Length == 10)
            {
                isbn13 = $"978{isbn10.Substring(0, 9)}";
                return String.Concat(isbn13, isbn13checksum(isbn13));
            }
            throw new ArgumentException($"{nameof(isbn)} değeri 10 karakterden oluşmalıdır!", nameof(isbn));
        }
        public static string Convert13to10(string isbn)
        {
            string isbn10, isbn13 = cleanisbn(isbn);
            if (isbn13.Length == 13)
            {
                isbn10 = isbn13.Substring(3, 9);
                return String.Concat(isbn10, isbn10checksum(isbn10));
            }
            throw new ArgumentException($"{nameof(isbn)} değeri 13 karakterden oluşmalıdır!", nameof(isbn));
        }
        public static bool IsValid(string isbn) => TryIsValid(isbn, out _);
        public static bool TryIsValid(string isbn, out string correctisbn)
        {
            isbn = cleanisbn(isbn);
            if (isbn.Length == 10) { return validateisbn10(isbn, out correctisbn); }
            if (isbn.Length == 13) { return validateisbn13(isbn, out correctisbn); }
            correctisbn = "";
            return false;
        }
        #region Private
        private string _isbn10;
        private string _isbn13;
        private void setisbn(string isbn)
        {
            isbn = cleanisbn(isbn);
            if (isbn.Length == 10)
            {
                this._isbn10 = isbn;
                this._isbn13 = Convert10to13(isbn);
            }
            else if (isbn.Length == 13)
            {
                this._isbn10 = Convert13to10(isbn);
                this._isbn13 = isbn;
            }
            else
            {
                this._isbn10 = "";
                this._isbn13 = "";
            }
        }
        private static string cleanisbn(string isbn) => isbn.ToStringOrEmpty().Replace("-", "").Replace(" ", "");
        private static string isbn10checksum(string isbn)
        {
            int i, rem, sum = 0;
            for (i = 0; i < 9; i++) { sum += ((10 - i) * Convert.ToInt32(isbn[i].ToString())); }
            rem = sum % 11;
            return rem == 0 ? "0" : (rem == 1 ? "X" : (11 - rem).ToString());
        }
        private static string isbn13checksum(string isbn)
        {
            int i, rem, sum = 0;
            for (i = 0; i < 12; i++) { sum += (((i % 2 == 0) ? 1 : 3) * Convert.ToInt32(isbn[i].ToString())); }
            rem = sum % 10;
            return rem == 0 ? "0" : (10 - rem).ToString();
        }
        private static bool validateisbn10(string isbn, out string correctISBN)
        {
            correctISBN = String.Concat(isbn.Substring(0, 9), isbn10checksum(isbn));
            return (correctISBN == isbn);
        }
        private static bool validateisbn13(string isbn, out string correctISBN)
        {
            correctISBN = String.Concat(isbn.Substring(0, 12), isbn13checksum(isbn));
            return (correctISBN == isbn);
        }
        #endregion
    }
}