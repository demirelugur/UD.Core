namespace UD.Core.Helper.Cryptographers
{
    using System;
    using System.Security.Cryptography;
    using System.Text;
    using UD.Core.Extensions;
    using UD.Core.Helper;
    public sealed class AESHelper
    {
        #region Private
        private const int _keyRequiredLength = 32;
        private const int _ivRequiredLength = 16;
        private static byte[] EncryptProcess(string plainText, Aes aes)
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
        private static string DecryptProcess(byte[] encryptedValue, byte[] key, byte[] iv)
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
        private static byte[] GenerateKey(string keyString, int requiredLength)
        {
            byte[] keyBytes = Encoding.UTF8.GetBytes(keyString.ToStringOrEmpty()), key = new byte[requiredLength];
            Array.Copy(keyBytes, key, Math.Min(keyBytes.Length, key.Length));
            return key;
        }
        #endregion
        public static string Encrypt(string plainText, string key)
        {
            using var aes = Aes.Create();
            aes.Key = GenerateKey(key, _keyRequiredLength);
            aes.GenerateIV();
            var encryptedData = EncryptProcess(plainText, aes);
            var result = new byte[_ivRequiredLength + encryptedData.Length];
            var offset = 0;
            Buffer.BlockCopy(aes.IV, 0, result, offset, _ivRequiredLength);
            offset += _ivRequiredLength;
            Buffer.BlockCopy(encryptedData, 0, result, offset, encryptedData.Length);
            return Convert.ToBase64String(result);
        }
        public static string Decrypt(string cipherText, string key)
        {
            var cipherBytes = Convert.FromBase64String(cipherText);
            if (cipherBytes.Length < _ivRequiredLength)
            {
                if (Checks.IsEnglishCurrentUICulture) { throw new ArgumentException("Invalid cipher text.", nameof(cipherText)); }
                throw new ArgumentException("Geçersiz şifreli metin.", nameof(cipherText));
            }
            var offset = 0;
            var keyBytes = GenerateKey(key, _keyRequiredLength);
            var ivBytes = new byte[_ivRequiredLength];
            Buffer.BlockCopy(cipherBytes, offset, ivBytes, 0, _ivRequiredLength);
            offset += _ivRequiredLength;
            var encryptedData = new byte[cipherBytes.Length - offset];
            Buffer.BlockCopy(cipherBytes, offset, encryptedData, 0, encryptedData.Length);
            return DecryptProcess(encryptedData, keyBytes, ivBytes);
        }
    }
}