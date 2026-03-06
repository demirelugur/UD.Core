namespace UD.Core.Extensions
{
    using Microsoft.EntityFrameworkCore;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Net;
    public static class ExceptionExtensions
    {
        /// <summary>Verilen istisnanın en içteki (inner) istisnasını döner.</summary>
        /// <param name="exception">İşlem yapılacak istisna.</param>
        /// <returns>En içteki istisna.</returns>
        public static Exception InnerEx(this Exception exception) => (exception.InnerException == null ? exception : exception.InnerException.InnerEx());
        /// <summary>Verilen bir istisna (exception) nesnesine göre uygun HTTP durum kodunu döndüren bir genişletme yöntemidir. Belirli istisna türleri için önceden tanımlı HTTP durum kodları eşleştirilir; eşleşme bulunamazsa varsayılan durum kodu döndürülür.</summary>
        /// <param name="exception">HTTP durum kodunun belirleneceği istisna nesnesi.</param>
        /// <param name="defaultValue">Eşleşen bir durum kodu bulunamazsa döndürülecek varsayılan HTTP durum kodu (varsayılan olarak <see cref="HttpStatusCode.InternalServerError"/>).</param>
        /// <returns>İstisna türüne karşılık gelen <see cref="HttpStatusCode"/> değeri.</returns>
        /// <remarks>
        /// Bu yöntem, aşağıdaki istisna türlerini destekler:
        /// <list type="bullet">
        /// <item><description><see cref="HttpRequestException"/> (<see cref="HttpRequestException.StatusCode"/> mevcutsa): İlgili durum kodu</description></item>
        /// <item><description><see cref="WebException"/> (<see cref="HttpWebResponse"/> mevcutsa): İlgili durum kodu</description></item>
        /// <item><description><see cref="UnauthorizedAccessException"/>: <see cref="HttpStatusCode.Unauthorized"/></description></item>
        /// <item><description><see cref="ArgumentException"/>: <see cref="HttpStatusCode.BadRequest"/></description></item>
        /// <item><description><see cref="TimeoutException"/>: <see cref="HttpStatusCode.RequestTimeout"/></description></item>
        /// <item><description><see cref="InvalidOperationException"/>: <see cref="HttpStatusCode.Conflict"/></description></item>
        /// </list>
        /// </remarks>
        public static HttpStatusCode GetHttpStatusCode(this Exception exception, HttpStatusCode defaultValue = HttpStatusCode.InternalServerError)
        {
            if (exception is HttpRequestException _hre && _hre.StatusCode.HasValue) { return _hre.StatusCode.Value; }
            if (exception is WebException _we && _we.Response is HttpWebResponse _hwr) { return _hwr.StatusCode; }
            if (exception is UnauthorizedAccessException) { return HttpStatusCode.Unauthorized; }
            if (exception is ArgumentException) { return HttpStatusCode.BadRequest; }
            if (exception is TimeoutException) { return HttpStatusCode.RequestTimeout; }
            if (exception is InvalidOperationException) { return HttpStatusCode.Conflict; }
            return defaultValue;
        }
        /// <summary>Belirtilen hatanın ve varsa iç içe geçmiş tüm hata nesnelerinin bir yığın (Stack) olarak döndürülmesini sağlar. Bu yöntem, hata zincirindeki tüm Exception nesnelerini elde etmenize olanak tanır.</summary>
        /// <param name="exception">Kök hata (Exception) nesnesi.</param>
        /// <returns>Exception nesnelerinden oluşan bir yığın (Stack).</returns>
        public static Stack<Exception> AllException(this Exception exception)
        {
            var stack = new Stack<Exception>();
            do
            {
                stack.Push(exception);
                exception = exception.InnerException;
            } while (exception != null);
            return stack;
        }
        /// <summary>Belirtilen hatanın ve varsa iç içe geçmiş tüm hata nesnelerinin mesajlarını bir dizi (string[]) olarak döndürür. Bu yöntem, hata zincirindeki her bir Exception nesnesinin mesajına erişim sağlar.</summary>
        /// <param name="exception">Kök hata (Exception) nesnesi.</param>
        /// <returns>Hata mesajlarından oluşan bir string dizisi.</returns>
        public static string[] AllExceptionMessage(this Exception exception) => exception.AllException().Select(x => x.Message).ToArray();
        /// <summary>Verilen bir istisna (Exception) nesnesinden, veritabanı varlık doğrulama hatalarını alır ve her bir hata için özellik adı (property name) ile hata mesajını içeren bir nesne dizisi döndürür. Eğer istisna bir <see cref="DbUpdateException"/> türünde ise, bu istisnanın Entries koleksiyonundaki her bir varlık için doğrulama işlemi gerçekleştirilir. Doğrulama hataları, özellik adı ve hata mesajı olarak anonim nesneler şeklinde listelenir. Herhangi bir hata oluşursa veya doğrulama hataları bulunamazsa, boş bir dizi döndürülür.</summary>
        /// <param name="exception">Hata bilgilerinin alınacağı istisna nesnesi.</param>
        /// <returns>Doğrulama hatalarını içeren anonim nesnelerden <c>(tableFullName: string, propertyName: string, errorMessage: string)</c> oluşan bir dizi. Hata yoksa veya işlem başarısız olursa boş bir dizi döner.</returns>
        public static object[] GetDbEntityValidationException(this Exception exception)
        {
            try
            {
                var r = new List<object>();
                if (exception is DbUpdateException _due)
                {
                    foreach (var entry in _due.Entries)
                    {
                        if (entry?.Entity == null) { continue; }
                        var vrs = new List<ValidationResult>();
                        Validator.TryValidateObject(entry.Entity, new(entry.Entity), vrs, true);
                        var tableFullName = entry.Entity.GetType().FullName;
                        foreach (var item in vrs)
                        {
                            if (item.ErrorMessage.IsNullOrEmpty()) { continue; }
                            foreach (var propertyName in item.MemberNames)
                            {
                                r.Add(new
                                {
                                    tableFullName,
                                    propertyName,
                                    errorMessage = item.ErrorMessage
                                });
                            }
                        }
                    }
                }
                return r.ToArray();
            }
            catch { return []; }
        }
    }
}