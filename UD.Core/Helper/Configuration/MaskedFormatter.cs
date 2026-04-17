namespace UD.Core.Helper.Configuration
{
    using UD.Core.Enums;
    using UD.Core.Extensions;
    using static UD.Core.Helper.GlobalConstants;
    public sealed class MaskedFormatter
    {
        /// <summary>Kimlik kartı veya nüfus cüzdanı seri numarasını maskeleme işlemi yapar. İsteğe bağlı olarak kimlik türü ve dil bilgisi ile birlikte açıklama ekler.</summary>
        /// <param name="serialNumber">Maske uygulanacak cüzdan seri numarası.</param>
        /// <param name="nVIIdentityCardTypes">Kimlik türü (yeni kimlik kartı veya eski nüfus cüzdanı).</param>
        /// <param name="showFull"><see langword="true"/> ise seri numarasının tamamı döner; <see langword="false"/> ise ilk 3 hane açık, kalan kısım * ile gizlenmiş döner.</param>
        /// <returns>Maske uygulanmış cüzdan seri numarası, opsiyonel olarak kimlik türü bilgisiyle birlikte. Geçersizse boş string döner.</returns>
        public static string SerialNumber(string serialNumber, EnumNVIIdentityCard? nVIIdentityCardTypes, bool showFull)
        {
            var cs = serialNumber.ToStringOrEmpty();
            if (cs == "") { return ""; }
            if (!showFull) { cs = String.Concat(cs.Substring(0, 3), new('*', cs.Length - 3)); }
            return (nVIIdentityCardTypes.HasValue ? $"{cs} ({nVIIdentityCardTypes.Value.GetLocalizedDescriptionFromEnum()})" : cs);
        }
        /// <summary>Doğum tarihini maskeler veya tam tarih olarak döndürür.</summary>
        /// <param name="date">Maskelenecek veya gösterilecek doğum tarihi.</param>
        /// <param name="showFull">Eğer <see langword="true"/> ise doğum tarihi tam olarak (dd.MM.yyyy) döner. Eğer <see langword="false"/> ise tarih maskelenir: gün olduğu gibi, ay &#39;**&#39; ile gizlenir, yılın ilk iki hanesi gösterilir ve son iki hanesi &#39;**&#39; ile gizlenir</param>
        /// <returns>Maskelenmiş veya tam doğum tarihi stringi.</returns>
        public static string BirthDate(DateOnly date, bool showFull)
        {
            var d = date.ToString(DateConstants.ddMMyyyy);
            if (showFull) { return d; }
            return $"{d.Substring(0, 2)}.**.{d.Substring(6, 2)}**";
        }
        /// <summary>Türkiye biçimine uygun telefon numarasını maskeleme işlemi yapar.</summary>
        /// <param name="phoneNumberTR">Maske uygulanacak telefon numarası (ülke kodu dahil).</param>
        /// <param name="showFull"><see langword="true"/> ise numara güzelleştirilmiş (biçimlenmiş) haliyle döner; <see langword="false"/> ise numaranın bazı bölümleri * ile gizlenmiş şekilde döner.</param>
        /// <returns>Maske uygulanmış veya tam telefon numarası. Geçersiz ise boş string döner.</returns>
        public static string PhoneNumberTR(string phoneNumberTR, bool showFull)
        {
            if (showFull) { return phoneNumberTR.ToPrettyPhoneNumberTR(); }
            return (TryValidators.TryPhoneNumberTR(phoneNumberTR, out string _t) ? $"(**{_t.Substring(2, 1)}) {_t.Substring(3, 1)}**-*{_t.Substring(8, 2)}" : "");
        }
        /// <summary>Verilen sayısal kimlik numarasını (TCKN veya VKN) maskeler. TCKN olarak doğrulanırsa orta kısım 6 adet &#39;*&#39;, VKN olarak doğrulanırsa 5 adet &#39;*&#39; ile gizlenir. Eğer <paramref name="showFull"/> true ise numara olduğu gibi döndürülür. Geçerli bir TCKN veya VKN değilse boş string döndürülür.</summary>
        /// <param name="identityNumber">Maskelenecek kimlik numarası.</param>
        /// <param name="showFull">true ise maskesiz tam numara döndürülür; false ise ilgili kısım maskelenir.</param>
        /// <returns>Maskelenmiş veya tam kimlik numarası. Geçerli bir TCKN/VKN değilse boş string döner.</returns>
        public static string TRTaxIdentityNumber(long identityNumber, bool showFull)
        {
            var count = 0;
            if (identityNumber.IsTRIdentityNumber()) { count = 6; }
            else if (identityNumber.IsTRTaxIdentityNumber()) { count = 5; }
            if (count == 0) { return ""; }
            var t = identityNumber.ToString();
            if (showFull) { return t; }
            return String.Concat(t.Substring(0, 3), new('*', count), t.Substring(9, 2));
        }
    }
}