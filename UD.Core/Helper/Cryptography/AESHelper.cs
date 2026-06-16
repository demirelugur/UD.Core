namespace UD.Core.Helper.Cryptography
{
    using System;
    using System.Security.Cryptography;
    using System.Text;
    using UD.Core.Extensions;
    using UD.Core.Helper;
    using UD.Core.Helper.Validation;
    public sealed class AESHelper
    {
        #region Private
        private const int keyRequiredLength = 32;
        private const int ivRequiredLength = 16;
        private static readonly byte[] randomNumbers = [4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16]; // ObfuscatorEncrypt işleminde oluşan şifreden bağımsız olarak belirli bir rastgele karakter kümesi oluşturur.
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
            byte[] keyBytes = Encoding.UTF8.GetBytes(keyString), key = new byte[requiredLength];
            Array.Copy(keyBytes, key, Math.Min(keyBytes.Length, key.Length));
            return key;
        }
        #endregion
        /// <summary><paramref name="plainText"/> metnini AES algoritması kullanarak şifreler. <paramref name="key"/> parametresi sağlanmazsa, rastgele bir anahtar oluşturulur ve şifreli metnin başına eklenir. Şifreli metin, anahtar (varsa) ve IV&#39;yi içeren bir byte dizisi olarak oluşturulur ve Base64 biçimine döndürülür.</summary>
        /// <param name="plainText">Şifrelenecek düz metin.</param>
        /// <param name="key">Opsiyonel. Şifreleme anahtarı. Sağlanmazsa rastgele bir anahtar oluşturulur. 16 karakter uzunluğunda bir anahtar olmalıdır!</param>
        /// <returns>Şifrelenmiş metin, Base64 biçiminde.</returns>
        public static string Encrypt(string plainText, string key = "")
        {
            Guard.ThrowIfEmpty(plainText, nameof(plainText));
            using var aes = Aes.Create();
            var keyIsEmpty = key.IsNullOrEmpty();
            if (keyIsEmpty) { aes.GenerateKey(); }
            else { aes.Key = generateKey(key, keyRequiredLength); }
            aes.GenerateIV();
            var encryptedData = encryptProcess(plainText, aes);
            var preLength = (keyIsEmpty ? keyRequiredLength : 0) + ivRequiredLength;
            var result = new byte[preLength + encryptedData.Length];
            var offset = 0;
            if (keyIsEmpty)
            {
                Buffer.BlockCopy(aes.Key, 0, result, offset, keyRequiredLength);
                offset += keyRequiredLength;
            }
            Buffer.BlockCopy(aes.IV, 0, result, offset, ivRequiredLength);
            offset += ivRequiredLength;
            Buffer.BlockCopy(encryptedData, 0, result, offset, encryptedData.Length);
            return Convert.ToBase64String(result);
        }
        /// <summary><paramref name="cipherText"/> parametresini AES algoritması kullanarak çözer. Şifreli metin, anahtar (varsa) ve IV&#39;yi içeren bir byte dizisi olarak Base64 biçiminde sağlanır. Eğer <paramref name="key"/> sağlanmazsa, şifreli metnin başından anahtar çıkarılır. IV her zaman şifreli metnin belirli bir bölümünde bulunur. Şifreleme işlemi sırasında kullanılan anahtar ve IV ile şifreli metin çözümlenir ve düz metin olarak döndürülür.</summary>
        /// <param name="cipherText">Çözülecek şifreli metin, Base64 biçiminde.</param>
        /// <param name="key">Opsiyonel. Şifreleme anahtarı. Sağlanmazsa şifreli metnin başından anahtar çıkarılır.</param>
        /// <returns>Çözümlenmiş düz metin.</returns>
        /// <exception cref="ArgumentException">Geçersiz şifreli metin sağlanırsa fırlatılır.</exception>
        public static string Decrypt(string cipherText, string key = "")
        {
            Guard.ThrowIfEmpty(cipherText, nameof(cipherText));
            var cipherBytes = Convert.FromBase64String(cipherText);
            var keyIsEmpty = key.IsNullOrEmpty();
            var minLength = (keyIsEmpty ? keyRequiredLength : 0) + ivRequiredLength;
            if (cipherBytes.Length < minLength)
            {
                if (Checks.IsEnglishCurrentUICulture) { throw new ArgumentException("Invalid cipher text.", nameof(cipherText)); }
                throw new ArgumentException("Geçersiz şifreli metin.", nameof(cipherText));
            }
            var offset = 0;
            var keyBytes = (keyIsEmpty ? new byte[keyRequiredLength] : generateKey(key, keyRequiredLength));
            if (keyIsEmpty)
            {
                Buffer.BlockCopy(cipherBytes, offset, keyBytes, 0, keyRequiredLength);
                offset += keyRequiredLength;
            }
            var ivBytes = new byte[ivRequiredLength];
            Buffer.BlockCopy(cipherBytes, offset, ivBytes, 0, ivRequiredLength);
            offset += ivRequiredLength;
            var encryptedData = new byte[cipherBytes.Length - offset];
            Buffer.BlockCopy(cipherBytes, offset, encryptedData, 0, encryptedData.Length);
            return decryptProcess(encryptedData, keyBytes, ivBytes);
        }
        /// <summary><paramref name="plainText"/> metnini AES algoritması kullanarak şifreler ve şifreli metni rastgele bir ön ekle karıştırarak daha karmaşık hale getirir. Şifreli metin, rastgele ön ek, şifrelenmiş metin ve ön ek uzunluğunu içeren bir byte dizisi olarak oluşturulur ve Base64 biçimine döndürülür. Bu yöntem, şifreli metnin yapısını gizleyerek güvenliği artırır.</summary>
        /// <param name="plainText">Şifrelenecek düz metin.</param>
        /// <returns>Şifrelenmiş ve karıştırılmış metin, Base64 biçiminde.</returns>
        public static string ObfuscatorEncrypt(string plainText)
        {
            Guard.ThrowIfEmpty(plainText, nameof(plainText));
            var randomPrefixLength = randomNumbers[Random.Shared.Next(randomNumbers.Length)];
            var randomPrefixBytes = RandomNumberGenerator.GetBytes(randomPrefixLength);
            var encryptedTextBytes = Encoding.UTF8.GetBytes(Converters.ToReverse(Encrypt(plainText)));
            var combinedBytes = new byte[randomPrefixLength + encryptedTextBytes.Length + 1];
            Buffer.BlockCopy(randomPrefixBytes, 0, combinedBytes, 0, randomPrefixLength);
            Buffer.BlockCopy(encryptedTextBytes, 0, combinedBytes, randomPrefixLength, encryptedTextBytes.Length);
            combinedBytes[combinedBytes.Length - 1] = randomPrefixLength;
            var base64 = Convert.ToBase64String(combinedBytes);
            var firstChar = base64[0];
            return String.Concat(firstChar.ToString(), CaesarHelper.Encrypt(base64.Substring(1), Convert.ToInt32(firstChar)));
        }
        /// <summary><paramref name="cipherText"/> parametresini AES algoritması kullanarak çözer. Şifreli metin, rastgele ön ek, şifrelenmiş metin ve ön ek uzunluğunu içeren bir byte dizisi olarak Base64 biçiminde sağlanır. Bu yöntem, şifreli metnin yapısını gizleyerek güvenliği artırır.</summary>
        /// <param name="cipherText">Çözülecek şifreli metin.</param>
        /// <returns>Çözülmüş düz metin.</returns>
        public static string ObfuscatorDecrypt(string cipherText)
        {
            Guard.ThrowIfEmpty(cipherText, nameof(cipherText));
            var firstChar = cipherText[0];
            var decoded = Convert.FromBase64String(String.Concat(firstChar.ToString(), CaesarHelper.Decrypt(cipherText.Substring(1), Convert.ToInt32(firstChar))));
            var randomPrefixLength = decoded[decoded.Length - 1];
            if (decoded.Length <= (randomPrefixLength + 1))
            {
                if (Checks.IsEnglishCurrentUICulture) { throw new ArgumentException("Invalid cipher text.", nameof(cipherText)); }
                throw new ArgumentException("Geçersiz şifreli metin.", nameof(cipherText));
            }
            var encryptedBytesLength = decoded.Length - randomPrefixLength - 1;
            var encryptedText = Encoding.UTF8.GetString(decoded, randomPrefixLength, encryptedBytesLength);
            return Decrypt(Converters.ToReverse(encryptedText));
        }
    }
}