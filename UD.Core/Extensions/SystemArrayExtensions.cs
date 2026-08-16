namespace UD.Core.Extensions
{
    using System;
    using System.Linq;
    using System.Security.Cryptography;
    using System.Text;
    using UD.Core.Helper;
    using UD.Core.Helper.Managements.Files;
    public static class SystemArrayExtensions
    {
        /// <summary>Hata mesajları dizisini iç içe geçmiş istisnalara dönüştürür.</summary>
        /// <param name="errors">Hata mesajlarının yer aldığı dizi.</param>
        /// <returns>İç içe geçmiş <see cref="Exception"/> nesnesi.</returns>
        public static Exception ToNestedException(this string[] errors)
        {
            errors = (errors ?? []).Reverse().ToArray();
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
        /// <summary>
        /// <paramref name="source"/> veri kümesinin <see cref="SHA512"/> veya <see cref="SHA256"/> karmasını hesaplar ve hexadecimal biçiminde bir dize olarak döndürür.
        /// </summary>
        /// <param name="source">Hash değeri hesaplanacak byte dizisi.</param>
        /// <param name="is512"><see langword="true"/> ise <see cref="SHA512"/>, false ise <see cref="SHA256"/> kullanılır.</param>
        /// <returns>Hexadecimal biçiminde hash değeri.</returns>
        public static string ComputeHash(this byte[] source, bool is512)
        {
            source ??= [];
            var hashBytes = is512 ? SHA512.HashData(source) : SHA256.HashData(source);
            var sb = new StringBuilder(hashBytes.Length * 2);
            foreach (var item in hashBytes) { sb.Append(item.ToString("X2")); }
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
            FileHelper.DirectoryCreate(new FileInfo(physicallyPath).DirectoryName);
            using var fs = new FileStream(physicallyPath, FileMode.Append, FileAccess.Write, FileShare.None, 4096, true);
            await fs.WriteAsync(bytes.AsMemory(0, bytes.Length), cancellationToken);
            await fs.FlushAsync(cancellationToken);
            fs.Close();
        }
        #endregion
    }
}
