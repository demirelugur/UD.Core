namespace UD.Core.Helper
{
    using UD.Core.Extensions;
    using static UD.Core.Helper.GlobalConstants;
    public sealed class MaskedFormatter
    {
        /// <summary>Türkiye biçimine uygun telefon numarasını maskeleme işlemi yapar.</summary>
        /// <param name="phoneNumberTR">Maske uygulanacak telefon numarası (ülke kodu dahil).</param>
        /// <param name="showFull"><see langword="true"/> ise numara güzelleştirilmiş (biçimlenmiş) haliyle döner; <see langword="false"/> ise numaranın bazı bölümleri * ile gizlenmiş şekilde döner.</param>
        /// <returns>Maske uygulanmış veya tam telefon numarası. Geçersiz ise boş string döner.</returns>
        public static string PhoneNumberTR(string phoneNumberTR, bool showFull)
        {
            if (showFull) { return phoneNumberTR.ToPrettyPhoneNumberTR(); }
            return (TryValidators.TryPhoneNumberTR(phoneNumberTR, out var _t) ? $"(**{_t.Substring(2, 1)}) {_t.Substring(3, 1)}**-*{_t.Substring(8, 2)}" : "");
        }
        /// <summary>Verilen sayısal kimlik numarasını (TCKN veya VKN) maskeler. TCKN olarak doğrulanırsa orta kısım 6 adet &#39;*&#39;, VKN olarak doğrulanırsa 5 adet &#39;*&#39; ile gizlenir. Eğer <paramref name="showFull"/> true ise numara olduğu gibi döndürülür. Geçerli bir TCKN veya VKN değilse boş string döndürülür.</summary>
        /// <param name="identityNumber">Maskelenecek kimlik numarası.</param>
        /// <param name="showFull">true ise maskesiz tam numara döndürülür; false ise ilgili kısım maskelenir.</param>
        /// <returns>Maskelenmiş veya tam kimlik numarası. Geçerli bir TCKN/VKN değilse boş string döner.</returns>
        public static string TRIdentityNumber(long identityNumber, bool showFull)
        {
            if (identityNumber.IsTRIdentityNumber())
            {
                var t = identityNumber.ToString();
                return (showFull ? t : String.Concat(t.Substring(0, 3), new('*', 6), t.Substring(9, 2)));
            }
            if (identityNumber.IsTRTaxIdentityNumber())
            {
                var t = identityNumber.ToString().Replicate(MaximumLengthConstants.TRTaxIdentityNumber);
                return (showFull ? t : String.Concat(t.Substring(0, 3), new('*', 5), t.Substring(8, 2)));
            }
            return "";
        }
    }
}