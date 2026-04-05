namespace UD.Core.Extensions
{
    using System;
    using System.Drawing;
    using System.Drawing.Drawing2D;
    using System.Drawing.Imaging;
    using UD.Core.Helper;
    using UD.Core.Helper.Validation;
    public static class DrawingExtensions
    {
        /// <summary>Görüntüyü belirtilen biçimde bayt dizisine dönüştürür.</summary>
        /// <param name="image">Dönüştürülecek görüntü.</param>
        /// <param name="imageFormat">Görüntünün kaydedileceği biçim.</param>
        /// <returns>Görüntünün bayt dizisi temsilini döndürür.</returns>
        public static byte[] ToByteArray(this Image image, ImageFormat imageFormat)
        {
            Guard.ThrowIfNull(image, nameof(image));
            Guard.ThrowIfNull(imageFormat, nameof(imageFormat));
            using (var ms = new MemoryStream())
            {
                image.Save(ms, imageFormat);
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
            Guard.ThrowIfNull(image, nameof(image));
            if (size.IsEmpty)
            {
                if (Checks.IsEnglishCurrentUICulture) { throw new ArgumentException($"{nameof(size)} parameter must be valid!", nameof(size)); }
                throw new ArgumentException($"{nameof(size)} parametresi geçerli olmalıdır!", nameof(size));
            }
            var bm = new Bitmap(size.Width, size.Height); // Not: using kullanılırsa bitmap değerleri iletilmemekte
            using (var g = Graphics.FromImage(bm))
            {
                g.CompositingQuality = CompositingQuality.HighQuality;
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                g.SmoothingMode = SmoothingMode.HighQuality;
                g.DrawImage(image, 0, 0, size.Width, size.Height);
                return bm;
            }
        }
        /// <summary>Verilen yeni genişlik(width) değerine göre resmin yüksekliğini(height), en boy oranını koruyarak hesaplar.</summary>
        /// <param name="image">Orijinal resim nesnesi.</param>
        /// <param name="width">Yeni genişlik değeri (piksel cinsinden).</param>
        /// <returns>En boy oranı korunarak hesaplanan yeni yükseklik değeri (piksel cinsinden).</returns>
        public static int CalculateHeight(this Image image, int width)
        {
            Guard.ThrowIfNull(image, nameof(image));
            Guard.ThrowIfZeroOrNegative(width, nameof(width));
            return Convert.ToInt32(image.Height * (Convert.ToSingle(width) / image.Width));
        }
        /// <summary>Verilen yeni yükseklik(height) değerine göre resmin genişliğini(width), en boy oranını koruyarak hesaplar.</summary>
        /// <param name="image">Orijinal resim nesnesi.</param>
        /// <param name="height">Yeni yükseklik değeri (piksel cinsinden).</param>
        /// <returns>En boy oranı korunarak hesaplanan yeni genişlik değeri (piksel cinsinden).</returns>
        public static int CalculateWidth(this Image image, int height)
        {
            Guard.ThrowIfNull(image, nameof(image));
            Guard.ThrowIfZeroOrNegative(height, nameof(height));
            return Convert.ToInt32(image.Width * (Convert.ToSingle(height) / image.Height));
        }
    }
}