namespace UD.Core.Helper.Generates
{
    using Microsoft.IdentityModel.Tokens;
    using System;
    using System.Collections.Generic;
    using System.IdentityModel.Tokens.Jwt;
    using System.Security.Claims;
    using System.Text;
    using UD.Core.Extensions;
    using UD.Core.Helper.Validations;
    using static UD.Core.Helper.GlobalConstants;
    public sealed class Generator
    {
        /// <summary><paramref name="claims"/> değerlerine göre bir JWT token oluşturur. Token, verilen <paramref name="key"/> ile imzalanır ve belirtilen süre boyunca geçerli olur. İsteğe bağlı olarak, token&#39;ın <paramref name="issuer"/> tarafından verildiği ve <paramref name="audience"/> tarafından hedeflendiği bilgileri de eklenebilir. Ayrıca, token&#39;ın geçerlilik başlangıç zamanı olarak <paramref name="notBefore"/> değeri de belirtilebilir.</summary>
        /// <param name="claims">JWT token&#39;ında yer alacak claim&#39;ler.</param>
        /// <param name="key">Token&#39;ı imzalamak için kullanılacak anahtar.</param>
        /// <param name="expiresIn">Token&#39;ın geçerlilik süresi.</param>
        /// <param name="issuer">Token&#39;ı oluşturan tarafın kimliği. Zorunlu alan değildir.</param>
        /// <param name="audience">Token&#39;ın hedef kitlesi. Zorunlu alan değildir.</param>
        /// <param name="notBefore">Token&#39;ın geçerli olmaya başlayacağı zaman.</param>
        /// <returns>Oluşturulan JWT token&#39;ı.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Geçersiz bir süre değeri verildiğinde fırlatılır.</exception>
        public static string WriteToken(IEnumerable<Claim> claims, string key, TimeSpan expiresIn, string? issuer = null, string? audience = null, DateTime? notBefore = null)
        {
            Guard.ThrowIfEmpty(claims, nameof(claims));
            Guard.ThrowIfEmpty(key, nameof(key));
            if (expiresIn <= TimeSpan.Zero)
            {
                var s = nameof(expiresIn);
                if (Checks.IsEnglishCurrentUICulture) { throw new ArgumentOutOfRangeException(s, $"{s} must be greater than zero."); }
                throw new ArgumentOutOfRangeException(s, $"{s} süresi sıfırdan büyük bir değer olmalıdır!");
            }
            var symmetricKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
            var creds = new SigningCredentials(symmetricKey, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(issuer.ParseOrDefault<string>(), audience.ParseOrDefault<string>(), claims, notBefore.NullOrDefault(), DateTime.UtcNow.Add(expiresIn), creds);
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        /// <summary>Kurallara uygun olarak sahte bir Türkiye Cumhuriyeti kimlik numarası üretir. Üretilen kimlik numarası, 11 haneli olup, ilk hanesi 0 olamaz ve son iki hanesi belirli bir algoritmaya göre hesaplanır. Bu metod, test ve geliştirme ortamlarında geçerli bir kimlik numarası gerektiren senaryolar için kullanılabilir.</summary>
        /// <returns>Oluşturulan sahte Türkiye Cumhuriyeti kimlik numarası.</returns>
        public static long FakeTRIdentityNumber()
        {
            Span<int> digits = stackalloc int[MaximumLengthConstants.TRIdentityNumber];
            var random = Random.Shared;
            digits[0] = random.Next(1, 10);
            int i;
            for (i = 1; i < 9; i++) { digits[i] = random.Next(10); }
            var oddSum = digits[0] + digits[2] + digits[4] + digits[6] + digits[8];
            var evenSum = digits[1] + digits[3] + digits[5] + digits[7];
            digits[9] = ((oddSum * 7) - evenSum) % 10;
            var firstTenSum = 0;
            for (i = 0; i < 10; i++) { firstTenSum += digits[i]; }
            digits[10] = firstTenSum % 10;
            return String.Concat(digits.ToArray()).ToLong();
        }
        /// <summary>Kurallara uygun olarak sahte bir Türkiye Cumhuriyeti vergi kimlik numarası üretir. Üretilen vergi kimlik numarası, 10 haneli olup, belirli bir algoritmaya göre hesaplanır. Bu metod, test ve geliştirme ortamlarında geçerli bir vergi kimlik numarası gerektiren senaryolar için kullanılabilir.</summary>
        /// <param name="significantDigits">Oluşturulacak vergi kimlik numarasının anlamlı basamak sayısı (1-10 arasında).</param>
        /// <returns>Oluşturulan sahte Türkiye Cumhuriyeti vergi kimlik numarası.</returns>
        public static long FakeTRTaxIdentityNumber(int significantDigits = MaximumLengthConstants.TRTaxIdentityNumber)
        {
            significantDigits = Math.Max(1, significantDigits);
            significantDigits = Math.Min(10, significantDigits);
            Span<int> digits = stackalloc int[MaximumLengthConstants.TRTaxIdentityNumber];
            int i, tmp, sum = 0, leadingZeros = 10 - significantDigits;
            for (i = 0; i < leadingZeros; i++) { digits[i] = 0; }
            var random = Random.Shared;
            for (i = leadingZeros; i < 9; i++) { digits[i] = random.Next(10); }
            for (i = 0; i < 9; i++)
            {
                tmp = (digits[i] + (9 - i)) % 10;
                if (tmp != 0)
                {
                    tmp = (tmp * Math.Pow(2, 9 - i)).ToInt32() % 9;
                    if (tmp == 0) { tmp = 9; }
                }
                sum += tmp;
            }
            digits[9] = (10 - (sum % 10)) % 10;
            return String.Concat(digits.Slice(leadingZeros, significantDigits).ToArray()).ToLong();
        }
    }
}