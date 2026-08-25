namespace UD.Core.Helper.Configurations
{
    using System;
    using UD.Core.Extensions;
    using UD.Core.Helper;
    using static UD.Core.Helper.GlobalConstants;
    public sealed class ISBNHelper
    {
        public ISBNHelper(string isbn) => this.SetISBN(isbn);
        public string Isbn10
        {
            get { return _isbn10; }
            set
            {
                if (TryIsValid(value, out string _c)) { this.SetISBN(_c); }
                if (Checks.IsEnglishCurrentUICulture) { throw new NotSupportedException("ISBN is incompatible!"); }
                throw new NotSupportedException($"{TitleConstants.Isbn} uyumsuzdur!");
            }
        }
        public string Isbn13
        {
            get { return _isbn13; }
            set
            {
                if (TryIsValid(value, out string _c)) { this.SetISBN(_c); }
                if (Checks.IsEnglishCurrentUICulture) { throw new NotSupportedException("ISBN is incompatible!"); }
                throw new NotSupportedException($"{TitleConstants.Isbn} uyumsuzdur!");
            }
        }
        public static string Convert10to13(string isbn)
        {
            string isbn13, isbn10 = CleanISBN(isbn);
            if (isbn10.Length == 10)
            {
                isbn13 = $"978{isbn10.Substring(0, 9)}";
                return String.Concat(isbn13, Isbn13Checksum(isbn13));
            }
            if (Checks.IsEnglishCurrentUICulture) { throw new ArgumentException($"{nameof(isbn)} value must be 10 characters long!", nameof(isbn)); }
            throw new ArgumentException($"{nameof(isbn)} değeri 10 karakterden oluşmalıdır!", nameof(isbn));
        }
        public static string Convert13to10(string isbn)
        {
            string isbn10, isbn13 = CleanISBN(isbn);
            if (isbn13.Length == 13)
            {
                isbn10 = isbn13.Substring(3, 9);
                return String.Concat(isbn10, Isbn10Checksum(isbn10));
            }
            if (Checks.IsEnglishCurrentUICulture) { throw new ArgumentException($"{nameof(isbn)} value must be 13 characters long!", nameof(isbn)); }
            throw new ArgumentException($"{nameof(isbn)} değeri 13 karakterden oluşmalıdır!", nameof(isbn));
        }
        public static bool IsValid(string isbn) => TryIsValid(isbn, out _);
        public static bool TryIsValid(string isbn, out string correctisbn)
        {
            isbn = CleanISBN(isbn);
            if (isbn.Length == 10) { return ValidateIsbn10(isbn, out correctisbn); }
            if (isbn.Length == 13) { return ValidateIsbn13(isbn, out correctisbn); }
            correctisbn = "";
            return false;
        }
        #region Private
        private string _isbn10;
        private string _isbn13;
        private void SetISBN(string isbn)
        {
            isbn = CleanISBN(isbn);
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
        private static string CleanISBN(string isbn) => isbn.RemoveMultipleSpace().Replace("-", "");
        private static string Isbn10Checksum(string isbn)
        {
            int i, rem, sum = 0;
            for (i = 0; i < 9; i++) { sum += ((10 - i) * Convert.ToInt32(isbn[i].ToString())); }
            rem = sum % 11;
            return rem == 0 ? "0" : (rem == 1 ? "X" : (11 - rem).ToString());
        }
        private static string Isbn13Checksum(string isbn)
        {
            int i, rem, sum = 0;
            for (i = 0; i < 12; i++) { sum += (((i % 2 == 0) ? 1 : 3) * Convert.ToInt32(isbn[i].ToString())); }
            rem = sum % 10;
            return rem == 0 ? "0" : (10 - rem).ToString();
        }
        private static bool ValidateIsbn10(string isbn, out string correctISBN)
        {
            correctISBN = String.Concat(isbn.Substring(0, 9), Isbn10Checksum(isbn));
            return (correctISBN == isbn);
        }
        private static bool ValidateIsbn13(string isbn, out string correctISBN)
        {
            correctISBN = String.Concat(isbn.Substring(0, 12), Isbn13Checksum(isbn));
            return (correctISBN == isbn);
        }
        #endregion
    }
}