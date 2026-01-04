namespace UD.Core.Extensions
{
    using Microsoft.AspNetCore.Http;
    using System;
    using System.Drawing;
    using System.Drawing.Drawing2D;
    using System.Drawing.Imaging;
    using System.IO;
    using UD.Core.Helper;
    using static UD.Core.Helper.OrtakTools;
    public static class IOExtensions
    {
        #region byte[]
        /// <summary>İkili verileri base64 biçiminde bir dizeye dönüştürür. <see cref="_to.ToBinaryFromBase64String(string, string)"/> işleminin tersidir</summary>
        /// <param name="bytes">Dönüştürülecek byte dizisi.</param>
        /// <param name="mimetype">Mime türü.</param>
        /// <returns>Base64 biçimindeki dize.</returns>
        public static string ToBase64StringFromBinary(this byte[] bytes, string mimetype) => $"data:{mimetype};base64,{Convert.ToBase64String(bytes)}";
        /// <summary>Verilen byte dizisini belirtilen fiziksel yola asenkron olarak yükler.</summary>
        public static async Task FileUploadAsync(this byte[] bytes, string physicallypath, CancellationToken cancellationtoken = default)
        {
            Guard.CheckEmpty(bytes, nameof(bytes));
            Guard.CheckEmpty(physicallypath, nameof(physicallypath));
            _file.DirectoryCreate(new FileInfo(physicallypath).DirectoryName);
            using (var fs = new FileStream(physicallypath, FileMode.Append, FileAccess.Write, FileShare.None, 4096, true))
            {
                await fs.WriteAsync(bytes.AsMemory(0, bytes.Length), cancellationtoken);
                await fs.FlushAsync(cancellationtoken);
                fs.Close();
            }
        }
        /// <summary>İki byte dizisinin içeriğinin aynı olup olmadığını kontrol eder. Diziler null ise boş dizi olarak kabul edilir. Eğer her iki dizi de boşsa, <see langword="true"/> döner. Dizilerin uzunlukları farklıysa, <see langword="false"/> döner. Elemanların sıralaması önemlidir; örneğin, [1, 2, 3] ve [1, 3, 2] dizileri aynı kabul edilmez. Her bir eleman sırayla karşılaştırılır ve herhangi bir farklılıkta <see langword="false"/> döner.</summary>
        /// <param name="file1">Karşılaştırılacak ilk byte dizisi.</param>
        /// <param name="file2">Karşılaştırılacak ikinci byte dizisi.</param>
        /// <returns>Diziler aynıysa <see langword="true"/>, farklıysa <see langword="false"/> döner.</returns>
        public static bool IsFileBytesEqual(this byte[] file1, byte[] file2)
        {
            file1 = file1 ?? Array.Empty<byte>();
            file2 = file2 ?? Array.Empty<byte>();
            if (file1.Length == 0 && file2.Length == 0) { return true; }
            if (file1.Length != file2.Length) { return false; }
            int i, _l = file1.Length;
            for (i = 0; i < _l; i++) { if (file1[i] != file2[i]) { return false; } }
            return true;
        }
        #endregion
        #region IFormFile
        /// <summary>Verilen IFormFile nesnesinden dosya adını döner (uzantısız).</summary>
        /// <param name="file">IFormFile nesnesi.</param>
        /// <returns>Dosya adı (uzantısız).</returns>
        public static string GetFileName(this IFormFile file) => Path.GetFileNameWithoutExtension(file.FileName);
        /// <summary>Verilen IFormFile nesnesinden dosya uzantısını döner (ilk karater &quot;.&quot; olacak biçimde)</summary>
        /// <param name="file">IFormFile nesnesi.</param>
        /// <returns>Dosya uzantısı (ilk karater &quot;.&quot; olacak biçimde).</returns>
        public static string GetFileExtension(this IFormFile file) => Path.GetExtension(file.FileName).ToLower();
        /// <summary>Verilen IFormFile nesnesini belirtilen fiziksel yola asenkron olarak yükler.</summary>
        public static async Task FileUploadAsync(this IFormFile file, string physicallypath, CancellationToken cancellationtoken = default)
        {
            Guard.CheckNull(file, nameof(file));
            Guard.CheckEmpty(physicallypath, nameof(physicallypath));
            _file.DirectoryCreate(new FileInfo(physicallypath).DirectoryName);
            using (var fs = new FileStream(physicallypath, FileMode.Create, FileAccess.Write, FileShare.None, 4096, true))
            {
                await file.CopyToAsync(fs, cancellationtoken);
            }
        }
        /// <summary>Bir IFormFile nesnesini byte dizisine dönüştürür.</summary>
        public static async Task<byte[]> ToByteArrayAsync(this IFormFile file, CancellationToken cancellationtoken = default)
        {
            Guard.CheckNull(file, nameof(file));
            using (var ms = new MemoryStream())
            {
                await file.CopyToAsync(ms, cancellationtoken);
                return ms.ToArray();
            }
        }
        #endregion
        #region Image
        /// <summary>Görüntüyü belirtilen biçimde bayt dizisine dönüştürür.</summary>
        /// <param name="image">Dönüştürülecek görüntü.</param>
        /// <param name="imageformat">Görüntünün kaydedileceği biçim.</param>
        /// <returns>Görüntünün bayt dizisi temsilini döndürür.</returns>
        public static byte[] ToByteArray(this Image image, ImageFormat imageformat)
        {
            using (var ms = new MemoryStream())
            {
                image.Save(ms, imageformat);
                return ms.ToArray();
            }
        }
        /// <summary>Görüntüyü belirtilen boyuta yeniden boyutlandırır.</summary>
        /// <param name="image">Yeniden boyutlandırılacak görüntü.</param>
        /// <param name="size">Hedef boyut.</param>
        /// <returns>Yeniden boyutlandırılmış Bitmap görüntüsünü döndürür.</returns>
        /// <exception cref="ArgumentException">Boyut parametresi geçersiz veya boş olduğunda fırlatılır.</exception>
        /// <remarks>Kullandığı yerde Dispose edilmelidir!</remarks>
        public static Bitmap Resize(this Image image, Size size)
        {
            if (size.IsEmpty) { throw new ArgumentException($"{nameof(size)} parametresi geçerli olmalıdır!", nameof(size)); }
            var _bm = new Bitmap(size.Width, size.Height); // Not: using kullanılırsa bitmap değerleri iletilmemekte
            using (var g = Graphics.FromImage(_bm))
            {
                g.CompositingQuality = CompositingQuality.HighQuality;
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                g.SmoothingMode = SmoothingMode.HighQuality;
                g.DrawImage(image, 0, 0, size.Width, size.Height);
                return _bm;
            }
        }
        /// <summary>Verilen yeni genişlik(width) değerine göre resmin yüksekliğini(height), en boy oranını koruyarak hesaplar.</summary>
        /// <param name="image">Orijinal resim nesnesi.</param>
        /// <param name="width">Yeni genişlik değeri (piksel cinsinden).</param>
        /// <returns>En boy oranı korunarak hesaplanan yeni yükseklik değeri (piksel cinsinden).</returns>
        public static int CalculateHeight(this Image image, int width) => Convert.ToInt32(image.Height * (Convert.ToSingle(width) / image.Width));
        /// <summary>Verilen yeni yükseklik(height) değerine göre resmin genişliğini(width), en boy oranını koruyarak hesaplar.</summary>
        /// <param name="image">Orijinal resim nesnesi.</param>
        /// <param name="height">Yeni yükseklik değeri (piksel cinsinden).</param>
        /// <returns>En boy oranı korunarak hesaplanan yeni genişlik değeri (piksel cinsinden).</returns>
        public static int CalculateWidth(this Image image, int height) => Convert.ToInt32(image.Width * (Convert.ToSingle(height) / image.Height));
        #endregion
        #region DirectoryInfo
        /// <summary>Belirtilen kaynak dizinini hedef dizine kopyalar.</summary>
        /// <param name="source">Kaynak dizini temsil eden DirectoryInfo nesnesi.</param>
        /// <param name="target">Hedef dizini temsil eden DirectoryInfo nesnesi.</param>
        public static void CopyAll(this DirectoryInfo source, DirectoryInfo target)
        {
            _file.DirectoryCreate(target.FullName);
            foreach (var item in source.GetFiles()) { item.CopyTo(Path.Combine(target.FullName, item.Name), true); }
            foreach (var item in source.GetDirectories()) { item.CopyAll(target.CreateSubdirectory(item.Name)); }
        }
        #endregion
    }
}