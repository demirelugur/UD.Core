namespace UD.Core.Helper.Cryptographers
{
    using System;
    using System.Security.Cryptography;
    using System.Text;
    using UD.Core.Extensions;
    using UD.Core.Helper;
    using UD.Core.Helper.Validations;
    public sealed class AESHelper
    {
        #region Private
        private const int keyRequiredLength = 32;
        private const int ivRequiredLength = 16;
        private static byte[] encryptProcess(string plainText, Aes aes)
        {
            using var ms = new MemoryStream();
            using var ce = aes.CreateEncryptor(aes.Key, aes.IV);
            using var cs = new CryptoStream(ms, ce, CryptoStreamMode.Write);
            using var sw = new StreamWriter(cs);
            sw.Write(plainText);
            sw.Flush();
            cs.FlushFinalBlock();
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
            byte[] keyBytes = Encoding.UTF8.GetBytes(keyString.ToStringOrEmpty()), key = new byte[requiredLength];
            Array.Copy(keyBytes, key, Math.Min(keyBytes.Length, key.Length));
            return key;
        }
        #endregion
        public static string Encrypt(string plainText, string key)
        {
            Guard.ThrowIfEmpty(plainText, nameof(plainText));
            using var aes = Aes.Create();
            aes.Key = generateKey(key, keyRequiredLength);
            aes.GenerateIV();
            var encryptedData = encryptProcess(plainText, aes);
            var result = new byte[ivRequiredLength + encryptedData.Length];
            var offset = 0;
            Buffer.BlockCopy(aes.IV, 0, result, offset, ivRequiredLength);
            offset += ivRequiredLength;
            Buffer.BlockCopy(encryptedData, 0, result, offset, encryptedData.Length);
            return Convert.ToBase64String(result);
        }
        public static string Decrypt(string cipherText, string key)
        {
            Guard.ThrowIfEmpty(cipherText, nameof(cipherText));
            var cipherBytes = Convert.FromBase64String(cipherText);
            if (cipherBytes.Length < ivRequiredLength)
            {
                if (Checks.IsEnglishCurrentUICulture) { throw new ArgumentException("Invalid cipher text.", nameof(cipherText)); }
                throw new ArgumentException("Geçersiz şifreli metin.", nameof(cipherText));
            }
            var offset = 0;
            var keyBytes = generateKey(key, keyRequiredLength);
            var ivBytes = new byte[ivRequiredLength];
            Buffer.BlockCopy(cipherBytes, offset, ivBytes, 0, ivRequiredLength);
            offset += ivRequiredLength;
            var encryptedData = new byte[cipherBytes.Length - offset];
            Buffer.BlockCopy(cipherBytes, offset, encryptedData, 0, encryptedData.Length);
            return decryptProcess(encryptedData, keyBytes, ivBytes);
        }
    }
}