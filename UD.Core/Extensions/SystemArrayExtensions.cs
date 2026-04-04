namespace UD.Core.Extensions
{
    using System;
    using System.Linq;
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
        /// <summary>İki byte dizisinin içeriğinin aynı olup olmadığını kontrol eder. Diziler null ise boş dizi olarak kabul edilir. Eğer her iki dizi de boşsa, <see langword="true"/> döner. Dizilerin uzunlukları farklıysa, <see langword="false"/> döner. Elemanların sıralaması önemlidir; örneğin, [1, 2, 3] ve [1, 3, 2] dizileri aynı kabul edilmez. Her bir eleman sırayla karşılaştırılır ve herhangi bir farklılıkta <see langword="false"/> döner.</summary>
        /// <param name="file1">Karşılaştırılacak ilk byte dizisi.</param>
        /// <param name="file2">Karşılaştırılacak ikinci byte dizisi.</param>
        /// <returns>Diziler aynıysa <see langword="true"/>, farklıysa <see langword="false"/> döner.</returns>
        public static bool IsFileBytesEqual(this byte[] file1, byte[] file2)
        {
            file1 ??= [];
            file2 ??= [];
            if (file1.Length == 0 && file2.Length == 0) { return true; }
            if (file1.Length != file2.Length) { return false; }
            int i, _l = file1.Length;
            for (i = 0; i < _l; i++) { if (file1[i] != file2[i]) { return false; } }
            return true;
        }
        #endregion
    }
}
