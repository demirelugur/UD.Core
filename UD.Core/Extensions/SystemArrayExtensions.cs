namespace UD.Core.Extensions
{
    using System;
    using System.Linq;
    using System.Security.Cryptography;
    using System.Text;
    using UD.Core.Helper;
    using UD.Core.Helper.Validation;
    public static class SystemArrayExtensions
    {
        /// <summary>Hata mesajları dizisini iç içe geçmiş istisnalara dönüştürür.</summary>
        /// <param name="errors">Hata mesajlarının yer aldığı dizi.</param>
        /// <returns>İç içe geçmiş <see cref="Exception"/> nesnesi.</returns>
        public static Exception ToNestedException(this string[] errors)
        {
            errors = (errors ?? []).Reverse().ToArray();
            Guard.ThrowIfEmpty(errors, nameof(errors));
            Exception ex = null;
            var i = errors.Length - 1;
            while (i >= 0)
            {
                if (ex == null) { ex = new(errors[i]); }
                else { ex = new(errors[i], ex); }
                i--;
            }
            return ex;
        }
        #region byte[]
        /// <summary><paramref name="source"/> dizisinin SHA256 hash&#39;ini hesaplar ve sonucu hexadecimal biçiminde bir dize olarak döndürür. Eğer <paramref name="source"/> null ise, boş bir dizi olarak kabul edilir ve hash değeri buna göre hesaplanır. Hash hesaplama işlemi, .NET&#39;in yerleşik SHA256 algoritması kullanılarak gerçekleştirilir. Sonuç olarak, döndürülen dize, her byte&#39;ın iki karakterle temsil edildiği hexadecimal biçiminde olacaktır.</summary>
        public static string ToSHA256Hexadecimal(this byte[] source)
        {
            var hashBytes = SHA256.HashData(source ?? []);
            var sb = new StringBuilder(hashBytes.Length * 2);
            foreach (var item in hashBytes) { sb.Append(item.ToString("x2")); }
            return sb.ToString();
        }
        /// <summary><paramref name="source"/> dizisinin SHA512 hash&#39;ini hesaplar ve sonucu hexadecimal biçiminde bir dize olarak döndürür. Eğer <paramref name="source"/> null ise, boş bir dizi olarak kabul edilir ve hash değeri buna göre hesaplanır. Hash hesaplama işlemi, .NET&#39;in yerleşik SHA512 algoritması kullanılarak gerçekleştirilir. Sonuç olarak, döndürülen dize, her byte&#39;ın iki karakterle temsil edildiği hexadecimal biçiminde olacaktır.</summary>
        public static string ToSHA512Hexadecimal(this byte[] source)
        {
            var hashBytes = SHA512.HashData(source ?? []);
            var sb = new StringBuilder(hashBytes.Length * 2);
            foreach (var item in hashBytes) { sb.Append(item.ToString("x2")); }
            return sb.ToString();
        }
        /// <summary>İkili verileri base64 biçiminde bir dizeye dönüştürür. <see cref="Converters.ToBinaryFromBase64String(string)"/> işleminin tersidir</summary>
        /// <param name="bytes">Dönüştürülecek byte dizisi.</param>
        /// <param name="mimeType">Mime türü.</param>
        /// <returns>Base64 biçimindeki dize.</returns>
        public static string ToBase64StringFromBinary(this byte[] bytes, string mimeType) => $"data:{mimeType};base64,{Convert.ToBase64String(bytes)}";
        /// <summary>Verilen byte dizisini belirtilen fiziksel yola asenkron olarak yükler.</summary>
        public static async Task FileUpload(this byte[] bytes, string physicallyPath, CancellationToken cancellationToken = default)
        {
            Guard.ThrowIfEmpty(bytes, nameof(bytes));
            Guard.ThrowIfEmpty(physicallyPath, nameof(physicallyPath));
            Files.DirectoryCreate(new FileInfo(physicallyPath).DirectoryName);
            using (var fs = new FileStream(physicallyPath, FileMode.Append, FileAccess.Write, FileShare.None, 4096, true))
            {
                await fs.WriteAsync(bytes.AsMemory(0, bytes.Length), cancellationToken);
                await fs.FlushAsync(cancellationToken);
                fs.Close();
            }
        }
        #endregion
    }
}
