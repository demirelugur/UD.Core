namespace UD.Core.Helper.Cryptography
{
    using System;
    using System.Security.Cryptography;
    using System.Text;
    using UD.Core.Helper;
    using UD.Core.Helper.Validation;
    public sealed class AESHelper
    {
        #region Private
        private const int keyRequiredLength = 32;
        private const int ivRequiredLength = 16;
        private static readonly byte[] randomNumbers = [4, 5, 6, 7, 8, 9, 10, 11, 12]; // ObfuscatorEncrypt işleminde oluşan şifreden bağımsız olarak belirli bir rastgele karakter kümesi oluşturur.
        private static byte[] encryptProcess(string value, Aes aes)
        {
            using (var ms = new MemoryStream())
            {
                using (var ce = aes.CreateEncryptor(aes.Key, aes.IV))
                {
                    using (var cs = new CryptoStream(ms, ce, CryptoStreamMode.Write))
                    {
                        using (var sw = new StreamWriter(cs))
                        {
                            sw.Write(value);
                            sw.Flush();
                            cs.FlushFinalBlock();
                            return ms.ToArray();
                        }
                    }
                }
            }
        }
        private static string decryptProcess(byte[] encryptedValue, byte[] key, byte[] iv)
        {
            using (var aes = Aes.Create())
            {
                aes.Key = key;
                aes.IV = iv;
                using (var cd = aes.CreateDecryptor(aes.Key, aes.IV))
                {
                    using (var ms = new MemoryStream(encryptedValue))
                    {
                        using (var cs = new CryptoStream(ms, cd, CryptoStreamMode.Read))
                        {
                            using (var sr = new StreamReader(cs))
                            {
                                return sr.ReadToEnd();
                            }
                        }
                    }
                }
            }
        }
        private static byte[] generateKey(string keyString, int requiredLength)
        {
            byte[] keyBytes = Encoding.UTF8.GetBytes(keyString), key = new byte[requiredLength];
            Array.Copy(keyBytes, key, Math.Min(keyBytes.Length, key.Length));
            return key;
        }
        #endregion
        public static string Encrypt(string value, string key, string iv)
        {
            Guard.ThrowIfEmpty(value, nameof(value));
            Guard.ThrowIfEmpty(key, nameof(key));
            Guard.ThrowIfEmpty(iv, nameof(iv));
            using (var aes = Aes.Create())
            {
                aes.Key = generateKey(key, keyRequiredLength);
                aes.IV = generateKey(iv, ivRequiredLength);
                return Convert.ToBase64String(encryptProcess(value, aes));
            }
        }
        public static string Decrypt(string encryptedValue, string key, string iv)
        {
            Guard.ThrowIfEmpty(encryptedValue, nameof(encryptedValue));
            Guard.ThrowIfEmpty(key, nameof(key));
            Guard.ThrowIfEmpty(iv, nameof(iv));
            return decryptProcess(Convert.FromBase64String(encryptedValue), generateKey(key, keyRequiredLength), generateKey(iv, ivRequiredLength));
        }
        public static string ObfuscatorEncrypt(string value)
        {
            using (var aes = Aes.Create())
            {
                aes.GenerateKey();
                aes.GenerateIV();
                using (var ms = new MemoryStream())
                {
                    var randomKeyLength = randomNumbers[Random.Shared.Next(randomNumbers.Length)];
                    foreach (var item in new[] {
                        Utilities.GenerateRandomKey(randomKeyLength), // randomkeylength değeri kadar rastgele karakter üretiyor
                        aes.Key,
                        aes.IV,
                        encryptProcess(value, aes), // Veri
                        [randomKeyLength] // Baştan kaç karakterin rastgele olduğunu belirten değer
                    }) { ms.Write(item.AsSpan()); }
                    var r = Convert.ToBase64String(ms.ToArray());
                    var firstChar = r[0]; // İlk değerin char değerine göre CaesarCipherOperation ile karıştırma
                    return Converters.ToReverse(String.Concat(firstChar.ToString(), Utilities.CaesarCipherOperation(r.Substring(1), Convert.ToInt32(firstChar))));
                }
            }
        }
        public static string ObfuscatorDecrypt(string encryptedValue)
        {
            Guard.ThrowIfEmpty(encryptedValue, nameof(encryptedValue));
            encryptedValue = Converters.ToReverse(encryptedValue);
            var firstChar = encryptedValue[0];
            var combinedBytes = Convert.FromBase64String(String.Concat(firstChar.ToString(), Utilities.CaesarCipherOperation(encryptedValue.Substring(1), -1 * Convert.ToInt32(firstChar))));
            var startIndex = combinedBytes[combinedBytes.Length - 1];
            var sourceSpan = combinedBytes.AsSpan();
            var encryptedStartIndex = startIndex + keyRequiredLength + ivRequiredLength;
            return decryptProcess(sourceSpan.Slice(encryptedStartIndex, combinedBytes.Length - (1 + encryptedStartIndex)).ToArray(), sourceSpan.Slice(startIndex, keyRequiredLength).ToArray(), sourceSpan.Slice(startIndex + keyRequiredLength, ivRequiredLength).ToArray());
        }
    }
}