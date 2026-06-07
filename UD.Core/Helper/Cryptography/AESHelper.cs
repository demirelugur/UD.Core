namespace UD.Core.Helper.Cryptography
{
    using System;
    using System.Security.Cryptography;
    using System.Text;
    using UD.Core.Extensions;
    using UD.Core.Helper.Validation;
    public sealed class AESHelper
    {
        #region Private
        private const int keyRequiredLength = 32;
        private const int ivRequiredLength = 16;
        private static byte[] encryptProcess(string value, Aes aes)
        {
            using var ms = new MemoryStream();
            using var ce = aes.CreateEncryptor(aes.Key, aes.IV);
            using var cs = new CryptoStream(ms, ce, CryptoStreamMode.Write);
            using (var sw = new StreamWriter(cs))
            {
                sw.Write(value);
            }
            return ms.ToArray();
        }
        private static string decryptProcess(byte[] encryptedValue, byte[] key, byte[] iv)
        {
            using var aes = Aes.Create();
            aes.Key = key;
            aes.IV = iv;
            using var cd = aes.CreateDecryptor(aes.Key, aes.IV);
            using var ms = new MemoryStream(encryptedValue);
            using var cs = new CryptoStream(ms, cd, CryptoStreamMode.Read);
            using var sr = new StreamReader(cs);
            return sr.ReadToEnd();
        }
        private static byte[] generateKey(string keyString, int requiredLength)
        {
            byte[] keyBytes = Encoding.UTF8.GetBytes(keyString), key = new byte[requiredLength];
            Array.Copy(keyBytes, key, Math.Min(keyBytes.Length, key.Length));
            return key;
        }
        #endregion
        public static string Encrypt(string plainText, string key, string iv = "")
        {
            Guard.ThrowIfEmpty(plainText, nameof(plainText));
            Guard.ThrowIfEmpty(key, nameof(key));
            using var aes = Aes.Create();
            aes.Key = generateKey(key, keyRequiredLength);
            var ivIsEmtpy = iv.IsNullOrEmpty();
            aes.IV = (ivIsEmtpy ? RandomNumberGenerator.GetBytes(ivRequiredLength) : generateKey(iv, ivRequiredLength));
            var encryptedData = encryptProcess(plainText, aes);
            if (!ivIsEmtpy) { return Convert.ToBase64String(encryptedData); }
            var result = new byte[aes.IV.Length + encryptedData.Length];
            Buffer.BlockCopy(aes.IV, 0, result, 0, aes.IV.Length);
            Buffer.BlockCopy(encryptedData, 0, result, aes.IV.Length, encryptedData.Length);
            return Convert.ToBase64String(result);
        }
        public static string Decrypt(string cipherText, string key, string iv = "")
        {
            Guard.ThrowIfEmpty(cipherText, nameof(cipherText));
            Guard.ThrowIfEmpty(key, nameof(key));
            var cipherBytes = Convert.FromBase64String(cipherText);
            var keyBytes = generateKey(key, keyRequiredLength);
            if (!iv.IsNullOrEmpty()) { return decryptProcess(cipherBytes, keyBytes, generateKey(iv, ivRequiredLength)); }
            if (cipherBytes.Length < ivRequiredLength)
            {
                if (Checks.IsEnglishCurrentUICulture) { throw new ArgumentException("Invalid cipher text.", nameof(cipherText)); }
                throw new ArgumentException("Geçersiz şifreli metin.", nameof(cipherText));
            }
            var ivBytes = new byte[ivRequiredLength];
            var encryptedData = new byte[cipherBytes.Length - ivRequiredLength];
            Buffer.BlockCopy(cipherBytes, 0, ivBytes, 0, ivRequiredLength);
            Buffer.BlockCopy(cipherBytes, ivRequiredLength, encryptedData, 0, encryptedData.Length);
            return decryptProcess(encryptedData, keyBytes, ivBytes);
        }
    }
}