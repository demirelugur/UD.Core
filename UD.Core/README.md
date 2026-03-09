# UD.Core

Kurumsal .NET 10 projeleri için tekrar kullanılabilir, doğrulama attribute'ları, yardımcı sınıflar, extension method'lar, middleware'ler ve servis entegrasyonları içeren kapsamlı bir kütüphanedir. Local NuGet paketi olarak dağıtılır.

## 🎯 Özellikler

- **✅ Gelişmiş Doğrulama Attribute'ları:** TCKN, VKN, e-Posta, ISBN, IP, MAC, Telefon, URL, Pozitif Sayılar, JSON, Array uzunluğu vb. için 15+ hazır validasyon
- **🛠️ Yardımcı Sınıflar:** Sahte Veri Üretimi (Bogus), Şifre Üretimi/Doğrulama, AES Şifreleme, Dosya İşlemleri, Dapper Entegrasyonu, E-posta Gönderimi vb.
- **🔧 Extension Method'lar:** 100+ extension method (Collection, String, DateTime, Numeric, HttpContext, FormCollection, Uri vb.)
- **🔐 Middleware'ler:** Transaction Management, Token Blacklist kontrolü
- **📊 Sonuç Tipleri:** ApiResult, ApiResult&lt;T&gt;, DateDiffResult, LeftJoinResult vb. standart yapılar
- **🗄️ Entity Framework Desteği:** Auditing (CreationAuditedEntity, FullAuditedEntity), DbContext Extensions, Transaction yönetimi

## 📦 Kurulum

Projenize local NuGet paketi olarak ekleyin veya DLL referansı verin.

## 🚀 Kullanım Örnekleri

### 1. Doğrulama Attribute'ları

UD.Core, model validasyonu için kapsamlı attribute koleksiyonu sunar:

```csharp
using UD.Core.Attributes.DataAnnotations;
using UD.Core.Attributes.DefaultValue;

public class KullaniciKayitModel
{
    // T.C. Kimlik No doğrulaması
    [UDTckn]
    public long? Tckn { get; set; }

    // Vergi Kimlik No doğrulaması  
    [UDVkn]
    public long? VergiNo { get; set; }

    // Zorunlu alan + String uzunluk kontrolü
    [UDRequired]
    [UDStringLength(100, 3)] // Min: 3, Max: 100 karakter
    public string AdSoyad { get; set; }

    // E-posta validasyonu
    [UDRequired]
    [EmailAddress(ErrorMessage = ValidationErrorMessageConstants.EMail)]
    [UDStringLength(MaximumLengthConstants.EMail)]
    public string Eposta { get; set; }

    // Türk telefon numarası (5XXXXXXXXX formatı)
    [UDPhoneNumberTR]
    public string? Telefon { get; set; }

    // URL validasyonu
    [UDUrl]
    [UDStringLength(500)]
    public string WebSite { get; set; }

    // IP Adresi validasyonu
    // IP Address varsayılan değer (IPAddress.Any)
    [UDIPAddress]
    [UDDefaultIPAddressAny]
    public string IpAddress { get; set; }

    // MAC Adresi validasyonu
    [UDMacAddress]
    public string? MacAddress { get; set; }

    // ISBN validasyonu
    [UDIsbn]
    public string? IsbnNo { get; set; }

    // Belirli değerler arasından seçim (Enum benzeri)
    [UDRequired]
    [UDIncludes(true, "aktif", "pasif", "beklemede")]
    [DefaultValue("aktif")]
    public string Durum { get; set; }

    // Pozitif Int16 kontrolü
    [UDRangePositiveInt16]
    public short Tip { get; set; }
    
    // Pozitif Int32 kontrolü
    [UDRangePositiveInt32]
    public int Yas { get; set; }

    // Pozitif Int64 kontrolü
    [UDRangePositiveInt64]
    public long SicilNo { get; set; }

    // Decimal hassasiyet kontrolü (Min: 0.01, Max: 999999.99, Precision: 8, Scale: 2)
    [UDRangeDecimalPrecision(0.01m, 8, 2)]
    public decimal Tutar { get; set; }

    // JSON formatı kontrolü
    [UDJson]
    public string? JsonData { get; set; }

    // Array minimum uzunluk kontrolü
    [UDArrayMinLength(2)]
    public string[] Yetkiler { get; set; }


    // Default değeri Guid.Empty
    [UDDefaultGuidEmpty]
    public Guid UId { get; set; }
}
```

### 2. Şifre Üretimi ve Doğrulama

Güçlü şifre üretimi ve doğrulama için kapsamlı araçlar:

```csharp
using UD.Core.Helper.Validation;

// Otomatik güçlü şifre üretme (8-16 karakter arası, büyük/küçük harf, rakam, noktalama)
var password = PasswordGenerator.Default.Generate();
Console.WriteLine($"Üretilen Şifre: {password}");

// Özelleştirilmiş şifre üretme
var customGenerator = new PasswordGenerator(
    upperCases: "ABCDEFGHIJKLMNOPQRSTUVWXYZ",
    lowerCases: "abcdefghijklmnopqrstuvwxyz", 
    digits: "0123456789",
    punctuations: "!@#$%^*"
);
var customPassword = customGenerator.Generate();

// Rastgele karakter dizisi üretme
var randomCode = PasswordGenerator.GenerateRandomChars(8); // 8 karakterlik kod
var numericCode = PasswordGenerator.GenerateRandomChars(6, "0123456789"); // 6 haneli sayısal kod

// Şifre güçlü mü kontrolü
var isStrong = PasswordGenerator.IsStrongPassword("MyPass123!", minimumLength: 8);

// Gelişmiş şifre doğrulama (Ad/Soyad, ardışık sayı, Türkçe karakter kontrolü)
var validator = new StrongPasswordValid(
    minimumLength: 8,
    maximumLength: 16,
    isConsecutive: true,      // 123, 987 gibi ardışık sayıları yasakla
    isEmpty: true,            // Boşluk karakteri olmasın
    isTurkishSpecialCharacter: true  // ş,ğ,ü,ö,ç,ı gibi karakterler olmasın
);

var testPassword = "Ahmet1234";
var hasWarning = validator.TryIsWarning(testPassword, "Ahmet", "Yılmaz", "tr", out string[] warnings);
if (hasWarning)
{
    Console.WriteLine("Şifre Uyarıları:");
    foreach (var warning in warnings)
    {
        Console.WriteLine($"- {warning}");
    }
}

// Varsayılan doğrulayıcı ile hızlı kontrol
var quickCheck = StrongPasswordValid.Default.TryIsWarning("Test123!", "", "", "en", out string[] errors);
```

### 3. TCKN ve VKN Doğrulama

```csharp
using UD.Core.Extensions;

// T.C. Kimlik No doğrulama
long tckn = 12345678901;
bool isTcknValid = tckn.IsTCKimlikNo();
Console.WriteLine($"TCKN Geçerli mi? {isTcknValid}");

// Vergi Kimlik No doğrulama
long vkn = 1234567890;
bool isVknValid = vkn.IsVergiKimlikNo();
Console.WriteLine($"VKN Geçerli mi? {isVknValid}");

// TCKN veya VKN kontrolü (ikisinden biri geçerliyse true)
bool isTcknOrVkn = tckn.IsTCKNorVKN();
```

### 4. Sahte Veri Üretimi (Bogus)

Test ve geliştirme için gerçekçi sahte veriler oluşturun:

```csharp
using UD.Core.Helper.Configuration;

// Basit kullanım
public record OgrenciBilgi(long tckn, string ogrenciNo, string ad, string soyad, string eposta);

var generator = new BogusGenericFakeDataGenerator(
    locale: "tr",           // Türkçe veriler
    nullChange: 0.1f,       // %10 null değer olasılığı
    arrayMinLength: 1,
    arrayMaxLength: 5
);

var ogrenciler = generator.GenerateArray<OgrenciBilgi>(10);

// Tek kayıt üretme
var tekOgrenci = generator.Generate<OgrenciBilgi>();

// Özelleştirilmiş aralıklarla
var customGenerator = new BogusGenericFakeDataGenerator()
    .WithIntegerRange(18, 65)           // Yaş aralığı
    .WithDecimalRange(1000m, 50000m)    // Maaş aralığı
    .WithDateTimeRange(
        new DateTime(2020, 1, 1), 
        DateTime.Now
    );

public record Personel(string ad, string soyad, int yas, decimal maas, DateTime iseGirisTarihi);
var personeller = customGenerator.GenerateArray<Personel>(50);
```

**Özel İşaretli Property Adları:**
- **String:** `seo`, `nms`, `src`, `ipaddress`, `color`, `mac`, `tel`, `adres`, `ad`, `name`, `soyad`, `surname`, `kuladi`, `username`, `eposta`, `email`
- **Int16:** `dahili`
- **Int64:** `tckn`, `vkn`

### 5. AES Şifreleme

```csharp
using UD.Core.Helper.Cryptography;

// Şifreleme
string plainText = "Gizli Veri";
string key = "MySecretKey123456789012345678901"; // 32 karakter
string iv = "MyInitVector1234"; // 16 karakter

string encrypted = AESHelper.Encrypt(plainText, key, iv);
Console.WriteLine($"Şifreli: {encrypted}");

// Çözme
string decrypted = AESHelper.Decrypt(encrypted, key, iv);
Console.WriteLine($"Çözülmüş: {decrypted}");

// Obfuscator şifreleme (Key/IV otomatik oluşturulur ve şifreye gömülür)
string obfuscated = AESHelper.ObfuscatorEncrypt("Hassas Bilgi");
string deobfuscated = AESHelper.ObfuscatorDecrypt(obfuscated);
```

### 6. Collection Extensions

```csharp
using UD.Core.Extensions;

// Null veya boş kontrol
var list = new List<int> { 1, 2, 3 };
bool hasItems = !list.IsNullOrCountZero();

// Koleksiyonu karıştır (Shuffle)
var shuffled = list.Shuffle();

// Dispose all
var disposableList = new List<FileStream> { /* ... */ };
disposableList.DisposeAll();

// Dictionary'ye ekle veya güncelle
var dict = new Dictionary<string, int>();
dict.AddOrUpdate("key1", 100);
dict.AddOrUpdate("key1", 200); // Günceller

// Şarta göre Dictionary'den sil
dict.RemoveWhere(x => x.Value < 150);

// Left Join (IEnumerable)
var leftList = new List<Customer> { /* ... */ };
var rightList = new List<Order> { /* ... */ };
var joined = leftList.LeftJoinEnumerable(
    rightList, 
    left => left.Id, 
    right => right.CustomerId
);

foreach (var item in joined)
{
    Console.WriteLine($"Customer: {item.left.Name}, Has Order: {item.hasRight}");
    if (item.hasRight)
    {
        Console.WriteLine($"Order: {item.right.OrderNo}");
    }
}

// ModelState hata ekleme
ModelStateDictionary modelState = /* ... */;
var errors = new[] { "Hata 1", "Hata 2" };
modelState.AddModelErrorRange(errors);
```

### 7. String Extensions

```csharp
using UD.Core.Extensions;

// SEO dostu URL oluşturma
string title = "C# ile Web Programlama!";
string seoUrl = title.ToSeoFriendly(); // "c-ile-web-programlama"

// Telefon numarası güzelleştirme
string phone = "5551234567";
string formatted = phone.BeautifyPhoneNumberTR(); // "(555) 123-4567"

// Null güvenli string işlemleri
string nullableStr = null;
string result = nullableStr.CoalesceOrDefault("Varsayılan", "Alternatif");
bool isEmpty = nullableStr.IsNullOrEmpty();
bool isWhiteSpace = nullableStr.IsNullOrWhiteSpace();

// Sayısal mı kontrolü
bool isNumeric = "12345".IsNumeric();

// Guid dönüşümü
Guid guid = "550e8400-e29b-41d4-a716-446655440000".ToGuid();

// Tarih dönüşümü
DateTime date = "2024-01-15".ToDate();
```

### 8. DateTime Extensions

```csharp
using UD.Core.Extensions;

var today = DateTime.Today;

// Ayın ilk ve son günü
var firstDay = today.GetFirstDayOfMonth();
var lastDay = today.GetLastDayOfMonth();

// Gün sonu (23:59:59.9999999)
var endOfDay = today.EndOfDay();

// Sonraki/önceki iş günü
var nextWorkDay = today.NextWorkDay();      // Cumartesi/Pazar atlanır
var previousWorkDay = today.PreviousWorkDay();

// Unix timestamp dönüşümleri
long unixMs = today.ToUnixTimestampMilliseconds();
var jsDate = unixMs.ToJsDate(); // JavaScript Date'e dönüş

// ISO 8601 format
string iso = DateTime.Now.ToISO8601(); // "2024-01-15T10:30:45.123Z"

// Gece yarısından itibaren geçen süre
int milliseconds = DateTime.Now.ToMillisecondsSinceMidnight(); // Max: 86399999
int seconds = DateTime.Now.ToSecondsSinceMidnight();           // Max: 86399
int minutes = DateTime.Now.ToMinutesSinceMidnight();           // Max: 1439

// SQL Server güvenli tarih
DateTime? safeDate = new DateTime(1700, 1, 1).SafeSqlDateTimeMin(); // null döner (1753'ten küçük)

// DateOnly dönüşümü
DateOnly dateOnly = today.ToDateOnly();

// OADate Integer
int oaDate = today.ToOADateInteger();
```

### 9. Numeric Extensions

```csharp
using UD.Core.Extensions;

// Asal sayı kontrolü
bool isPrime = 17UL.IsPrimeNumber(); // true

// Active Directory FileTime dönüşümü
long adFileTime = 132891234567890000;
DateTime? adDate = adFileTime.ToFileTimeUTC();
```

### 10. FormCollection Extensions

```csharp
using UD.Core.Extensions;
using Microsoft.AspNetCore.Http;

// Controller içinde
public IActionResult SubmitForm(IFormCollection form)
{
    // Tip güvenli değer okuma
    int id = form.ParseOrDefault<int>("id");
    int? nullableId = form.ParseOrDefault<int?>("nullableId");
    string email = form.ParseOrDefault<string>("email");
    DateTime tarih = form.ParseOrDefault<DateTime>("tarih");
    bool aktif = form.ParseOrDefault<bool>("aktif");

    // Dizi değerleri okuma (checkbox, multi-select vb.)
    if (form.TryGetArrayValue<int>("yetkiIds[]", out int[] yetkiIds))
    {
        // yetkiIds dizisi ile işlem yap
    }

    // String değer okuma
    if (form.TryGetStringValue("aciklama", out string aciklama))
    {
        // aciklama ile işlem yap
    }

    return Ok();
}
```

### 11. HttpContext Extensions

```csharp
using UD.Core.Extensions;
using Microsoft.AspNetCore.Http;

public class MyController : ControllerBase
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public IActionResult GetClientInfo()
    {
        var context = _httpContextAccessor.HttpContext;

        // Mobil cihaz mı?
        bool isMobile = context.IsMobileDevice();

        // Base URL
        Uri baseUri = context.GetBaseUri(); // https://example.com

        // Tam istek URL'i
        Uri fullUri = context.GetFullRequestUri(); // https://example.com/api/users?page=1

        // Bearer Token
        string token = context.GetToken();

        // İstemci IP Adresi (X-Forwarded-For destekli)
        var ipAddress = context.GetIPAddress();

        return Ok(new { isMobile, baseUri, fullUri, token, ipAddress });
    }
}
```

### 12. ApiResult Kullanımı

Standartlaştırılmış API yanıtları:

```csharp
using UD.Core.Helper.Results;

public class UserController : ControllerBase
{
    // Başarılı yanıt
    [HttpGet("{id}")]
    public IActionResult GetUser(int id)
    {
        var user = _userService.GetById(id);

        if (user == null)
        {
            return NotFound(ApiResult.setFailed("Kullanıcı bulunamadı"));
        }

        return Ok(new ApiResult<UserDto>(user, true, null));
    }

    // Hatalı yanıt
    [HttpPost]
    public IActionResult CreateUser(CreateUserDto dto)
    {
        if (!ModelState.IsValid)
        {
            var errors = ModelState.Values
                .SelectMany(v => v.Errors)
                .Select(e => e.ErrorMessage)
                .ToArray();

            return BadRequest(ApiResult.setFailed(errors));
        }

        var result = _userService.Create(dto);
        return Ok(new ApiResult<int>(result.Id, true, null));
    }

    // Başarılı (data olmadan)
    [HttpDelete("{id}")]
    public IActionResult DeleteUser(int id)
    {
        _userService.Delete(id);
        return Ok(ApiResult.setSuccess);
    }
}

// ApiResult yapısı:
// {
//   "status": true/false,
//   "errors": ["Hata 1", "Hata 2"],
//   "response": { ... } // ApiResult<T> kullanıldığında
// }
```

### 13. Dapper Helper

Entity Framework ile Dapper'ı birlikte kullanma:

```csharp
using UD.Core.Helper.Database;
using Microsoft.EntityFrameworkCore;

public class ReportService
{
    private readonly AppDbContext _context;

    public async Task<IEnumerable<ReportDto>> GetComplexReport(int year)
    {
        // DbContext'ten Dapper Helper oluştur (Transaction dahil)
        using var dapper = new DapperHelper(_context);

        var sql = @"
            SELECT 
                u.Name, 
                COUNT(o.Id) as OrderCount,
                SUM(o.Total) as TotalAmount
            FROM Users u
            LEFT JOIN Orders o ON u.Id = o.UserId
            WHERE YEAR(o.OrderDate) = @Year
            GROUP BY u.Name
        ";

        var result = await dapper.Query<ReportDto>(
            commandText: sql,
            parameters: new { Year = year },
            commandTimeout: 30,
            commandType: CommandType.Text
        );

        return result;
    }

    // Stored Procedure çağırma
    public async Task<int> ExecuteStoredProcedure()
    {
        using var dapper = new DapperHelper(_context);

        return await dapper.Execute(
            commandText: "sp_UpdateStatistics",
            parameters: new { UpdateDate = DateTime.Now },
            commandTimeout: null,
            commandType: CommandType.StoredProcedure
        );
    }

    // Multiple result sets
    public async Task<(IEnumerable<User> users, IEnumerable<Order> orders)> GetMultipleResults()
    {
        using var dapper = new DapperHelper(_context);

        var sql = "SELECT * FROM Users; SELECT * FROM Orders;";

        using var multi = await dapper.QueryMultiple(sql, null, null, CommandType.Text);

        var users = await multi.ReadAsync<User>();
        var orders = await multi.ReadAsync<Order>();

        return (users, orders);
    }
}
```

### 14. Transaction Middleware

Otomatik transaction yönetimi:

```csharp
// Startup.cs veya Program.cs
public void Configure(IApplicationBuilder app)
{
    // POST, PUT, PATCH, DELETE isteklerinde otomatik transaction başlat
    app.UseMiddleware<TransactionMiddleware<AppDbContext>>();

    app.UseRouting();
    app.UseEndpoints(endpoints => endpoints.MapControllers());
}

// Controller'da kullanım
public class ProductController : ControllerBase
{
    // Bu endpoint'te transaction otomatik yönetilir
    [HttpPost]
    public IActionResult Create(ProductDto dto)
    {
        _context.Products.Add(product);
        _context.SaveChanges(); // Middleware içinde commit/rollback yapılır

        return Ok();
    }

    // Transaction'ı devre dışı bırak
    [HttpPost("import")]
    [DisableTransaction] // Bu attribute ile middleware atlanır
    public async Task<IActionResult> BulkImport(List<ProductDto> products)
    {
        // Manuel transaction yönetimi yapabilirsiniz
        using var transaction = await _context.Database.BeginTransactionAsync();
        try
        {
            // Bulk işlemler...
            await transaction.CommitAsync();
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }

        return Ok();
    }
}
```

**Middleware Davranışı:**
- Sadece POST, PUT, PATCH, DELETE metodlarında çalışır
- HTTP 200-399 arası yanıtlarda commit yapar
- HTTP 400+ yanıtlarda rollback yapar
- `[DisableTransaction]` attribute'ü ile devre dışı bırakılabilir
- Entity Framework'ün retry stratejisini destekler

### 15. Token Blacklist Middleware

Token bazlı kimlik doğrulamada token iptal mekanizması:

```csharp
// Startup.cs veya Program.cs
public void ConfigureServices(IServiceCollection services)
{
    // TokenBlacklistService'i Singleton olarak kaydet
    services.AddSingleton<ITokenBlacklistService, TokenBlacklistService>();
}

public void Configure(IApplicationBuilder app)
{
    app.UseAuthentication();

    // Token blacklist kontrolü
    app.UseMiddleware<TokenBlacklistMiddleware>();

    app.UseAuthorization();
    app.UseRouting();
    app.UseEndpoints(endpoints => endpoints.MapControllers());
}

// Kullanım
public class AuthController : ControllerBase
{
    private readonly ITokenBlacklistService _tokenBlacklistService;

    // Logout işlemi
    [Authorize]
    [HttpPost("logout")]
    public async Task<IActionResult> Logout()
    {
        var token = HttpContext.GetToken();

        // Token'ı blacklist'e ekle (1 saat geçerli)
        await _tokenBlacklistService.Add(token, TimeSpan.FromHours(1));

        return Ok(ApiResult.setSuccess);
    }

    // Token kontrol
    [HttpGet("check-token")]
    public async Task<IActionResult> CheckToken()
    {
        var token = HttpContext.GetToken();
        var isBlacklisted = await _tokenBlacklistService.Any(token);

        return Ok(new { isValid = !isBlacklisted });
    }
}
```

**TokenBlacklistService Metodları:**
- `Task<bool> Any(string token)` - Token blacklist'te mi kontrol eder
- `Task Add(string token, TimeSpan expiration)` - Token'ı blacklist'e ekler
- `Task TryAdd(string token, TimeSpan expiration)` - Yoksa ekler, varsa günceller

**Middleware Davranışı:**
- Bearer token'ı otomatik olarak alır
- Blacklist'te varsa HTTP 401 Unauthorized döner
- Süresi dolan tokenlar otomatik temizlenir

### 16. E-posta Gönderimi

SMTP üzerinden e-posta gönderme:

```csharp
using UD.Core.Helper.MailManagement;

// Gmail ile e-posta gönderme
var gmailSettings = SmtpClientBasic.SetGmail("your-email@gmail.com", "your-app-password");
using var gmailClient = gmailSettings.toSmtpClient();

var mailMessage = new MailMessage
{
    From = new MailAddress(gmailSettings.Email),
    Subject = "Test E-posta",
    Body = "Bu bir test e-postasıdır.",
    IsBodyHtml = true
};
mailMessage.To.Add("recipient@example.com");

gmailClient.Send(mailMessage);

// Outlook ile e-posta gönderme
var outlookSettings = SmtpClientBasic.SetOutlook("your-email@outlook.com", "your-password");

// Özel SMTP ayarları
var customSmtp = new SmtpClientBasic(
    email: "info@mycompany.com",
    password: "password123",
    host: "smtp.mycompany.com",
    port: 587,
    enablessl: true,
    usedefaultcredentials: false,
    deliverymethod: SmtpDeliveryMethod.Network,
    timeout: 30000
);
```

### 17. Dosya Yükleme İşlemleri

Transaction-safe dosya yükleme:

```csharp
using UD.Core.Helper.FileManagement;

public class DocumentController : ControllerBase
{
    [HttpPost("upload")]
    public async Task<IActionResult> Upload(IFormFile file)
    {
        var fileProcess = new FileUploadProcess();

        // Eski dosyayı sil
        fileProcess.RemoveFile("/uploads/old-document.pdf");

        // Eski klasörü sil
        fileProcess.RemoveDirectory("/uploads/temp");

        // Yeni dosya ekle
        var newPath = $"/uploads/{Guid.NewGuid()}.pdf";
        fileProcess.Add(newPath, file);

        // Veya byte[] olarak
        byte[] fileBytes = await file.GetBytesAsync();
        fileProcess.Add(newPath, fileBytes);

        try
        {
            // Veritabanı işlemleri...
            _context.Documents.Add(new Document { Path = newPath });
            await _context.SaveChangesAsync();

            // Her şey başarılıysa dosya işlemlerini gerçekleştir
            await fileProcess.ProcessFileUploadsAndDeletions();

            return Ok();
        }
        catch
        {
            // Hata durumunda dosya işlemleri yapılmaz
            throw;
        }
    }
}
```

**FileUploadProcess Özellikleri:**
- Transaction-safe (hata durumunda dosya işlemleri geri alınabilir)
- Önce silme sonra ekleme mantığı
- IFormFile ve byte[] desteği
- Async işlem desteği

### 18. Auditing Entities

Otomatik oluşturma/güncelleme takibi:

```csharp
using UD.Core.Auditing;

// Sadece oluşturma bilgisi
public class Category : CreationAuditedEntity
{
    public string Name { get; set; }
    // CreatedAt, CreatedBy otomatik doldurulur
}

// Oluşturma ve güncelleme bilgisi
public class Product : AuditedEntity
{
    public string Name { get; set; }
    public decimal Price { get; set; }
    // CreatedAt, CreatedBy, UpdatedAt, UpdatedBy otomatik doldurulur
}

// Soft delete dahil tam audit
public class Order : FullAuditedEntity
{
    public string OrderNo { get; set; }
    public decimal Total { get; set; }
    // CreatedAt, CreatedBy, UpdatedAt, UpdatedBy, DeletedAt, DeletedBy, IsDeleted
}

// DbContext'te SaveChanges override
public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
{
    var currentUser = _httpContextAccessor.HttpContext?.User?.Identity?.Name ?? "System";
    var now = DateTime.UtcNow;

    foreach (var entry in ChangeTracker.Entries<IAuditedEntity>())
    {
        if (entry.State == EntityState.Added)
        {
            entry.Entity.CreatedAt = now;
            entry.Entity.CreatedBy = currentUser;
        }

        if (entry.State == EntityState.Modified)
        {
            entry.Entity.UpdatedAt = now;
            entry.Entity.UpdatedBy = currentUser;
        }
    }

    // Soft delete
    foreach (var entry in ChangeTracker.Entries<FullAuditedEntity>())
    {
        if (entry.State == EntityState.Deleted)
        {
            entry.State = EntityState.Modified;
            entry.Entity.IsDeleted = true;
            entry.Entity.DeletedAt = now;
            entry.Entity.DeletedBy = currentUser;
        }
    }

    return await base.SaveChangesAsync(cancellationToken);
}
```

### 19. Global Constants ve Validators

```csharp
using UD.Core.Helper;
using static UD.Core.Helper.GlobalConstants;

// Maximum uzunluk sabitleri
var tcknLength = MaximumLengthConstants.Tckn; // 11
var emailLength = MaximumLengthConstants.EMail; // 100
var uriLength = MaximumLengthConstants.Uri; // 500

// Tarih formatleri
string dateFormat = DateConstants.ddMMyyyy_HHmmss; // "dd.MM.yyyy HH:mm:ss"
string sqlMinDate = DateConstants.SqlMinValue; // "1753-01-01"

// Türkçe özel karakterler
char[] turkishChars = GlobalConstants.TurkishSpecialCharacters; // ş, ğ, ü, ö, ç, ı, İ

// CultureInfo alma
var trCulture = OrtakTools.Accessors.GetCultureInfo("tr");
var enCulture = OrtakTools.Accessors.GetCultureInfo("en");

// İsim kısaltmaları
string initials = OrtakTools.Accessors.GetNameInitials("Mustafa", "Kemal", "ATATÜRK"); // "MK.A"

// Connection string oluşturma
string connStr = OrtakTools.Accessors.GetConnectionString(
    dataSource: "localhost",
    initialCatalog: "MyDatabase",
    userID: "sa",
    password: "password123"
);

// Property uzunluk bilgisi alma
var nameLength = OrtakTools.Accessors.GetStringOrMaxLength<UserDto>("Name");
var emailMaxLength = OrtakTools.Accessors.GetStringOrMaxLength<UserDto>(x => x.Email);

// Guard validasyonları
Guard.CheckEmpty(value, nameof(value)); // Boş mu?
Guard.CheckZeroOrNegative(count, nameof(count)); // Sıfır veya negatif mi?
Guard.UnSupportLanguage(lang, nameof(lang)); // Desteklenen dil mi? (tr, en)
```

## 📚 Enum'lar

```csharp
using UD.Core.Enums;

// Türkçe Aylar
var month = CMonthTR.Ocak;

// Türkçe Haftanın Günleri
var day = CDayOfWeekTR.Pazartesi;

// API Mesaj Tipleri
var message = CRetMesaj.basarili;
message.GetDescriptionFromEnum(); // Açıklama metnini al

// NVI Kimlik Tipleri
var idType = NVIKimlikTypes.TCKimlikNo;

// Hata Öncelik Seviyeleri
var priority = ErrorPriorityTypes.Critical;
```

## 🔧 Gelişmiş Özellikler

### BaseService Sınıfları

Generic CRUD işlemleri için hazır service sınıfları:

```csharp
// Primary key'i olan entity'ler için
public class UserService : BaseServicePrimary<User, int, AppDbContext>
{
    public UserService(AppDbContext context) : base(context) { }

    // CRUD metodları hazır: GetById, GetAll, Add, Update, Delete vb.
}

// Composite key'li entity'ler için
public class UserRoleService : BaseServiceComplexKey<UserRole, AppDbContext>
{
    public UserRoleService(AppDbContext context) : base(context) { }
}

// Genel kullanım
public class ProductService : BaseService<Product, AppDbContext>
{
    public ProductService(AppDbContext context) : base(context) { }
}
```

## 🤝 Katkıda Bulunma

Bu proje kapalı kaynaklıdır ve şirket içi kullanım için geliştirilmiştir.

## 📧 İletişim

- **Geliştirici:** Uğur Demirel
- **E-posta:** [ugur.demirel@outlook.com](mailto:ugur.demirel@outlook.com)
- **GitHub:** [demirelugur/UD.Core](https://github.com/demirelugur/UD.Core)

## 📝 Lisans

Bu kütüphane telif hakkı koruması altındadır. Kullanım hakları şirket politikalarına tabidir.

---

**Not:** .NET 10 hedeflenmiştir. Minimum gereksinim: .NET 10 SDK