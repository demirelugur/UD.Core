# UD.Core

Kurumsal .NET 10 projeleri için tekrar kullanılabilir bileşenler içeren kapsamlı bir kütüphanedir. Local NuGet paketi olarak dağıtılır.

## 📦 Kurulum

Projenize local NuGet paketi olarak ekleyin veya DLL referansı verin.

```bash
dotnet add package UD.Core
```

## 🏗️ Proje Yapısı

### UD.Core.Attributes
Model validasyonu ve metadata tanımlamaları için attribute koleksiyonu. TCKN, VKN, e-Posta, Telefon, IP, MAC, ISBN gibi özel veri türleri için 15+ doğrulama attribute'ü ile default değer atama özelliklerini içerir.

### UD.Core.Auditing
Entity Framework için otomatik audit tracking altyapısı. Oluşturma, güncelleme ve soft delete işlemlerini kullanıcı ve tarih bilgisiyle izleyen base entity sınıfları sağlar.

### UD.Core.Enums
Proje genelinde kullanılan enum tipleri. API mesaj tipleri, hata öncelik seviyeleri, NVI kimlik tipleri ve çeşitli iş mantığı enumları içerir.

### UD.Core.Extensions
100+ extension method koleksiyonu. String, DateTime, Collection, Numeric, HttpContext, FormCollection, Uri, Expression ve daha fazla tip için yardımcı metodlar sunar.

### UD.Core.Helper
Yardımcı sınıflar ve Entity Framework Base Servis altyapısı. Şifre üretimi, AES şifreleme, Dosya işlemleri, e-Posta gönderimi, Dapper entegrasyonu, Bogus sahte veri üretimi, TCMB kur servisi ve API sonuç tipleri içerir.

### UD.Core.Middlewares
ASP.NET Core middleware bileşenleri. Transaction yönetimi, token blacklist kontrolü ve güvenlik header'ları için hazır middleware sınıfları sağlar.

## 📦 Temel Özellikler

- **✅ Gelişmiş Doğrulama:** TCKN, VKN, e-Posta, ISBN, IP, MAC, Telefon, URL, Pozitif Sayılar, JSON validasyonları
- **🔧 Extension Methods:** 100+ extension method (Collection, String, DateTime, Numeric, HttpContext vb.)
- **🛠️ Yardımcı Araçlar:** AES şifreleme, şifre üretimi, dosya işlemleri, e-posta gönderimi
- **🔐 Middleware'ler:** Transaction management, Token blacklist kontrolü, Security headers
- **📊 Sonuç Tipleri:** ApiResult, DateDiffResult, LeftJoinResult standart yapıları

## 🚀 Hızlı Başlangıç

### Validation Attributes
```csharp
using UD.Core.Attributes.DataAnnotations;

public class UserModel
{
    [UDTRIdentityNumber]
    public long? TRIdentityNumber { get; set; }

    [UDRequired]
    [UDEmail]
    public string Email { get; set; }

    [UDPhoneNumberTR]
    public string? Phone { get; set; }
}
```

### Extension Methods
```csharp
using UD.Core.Extensions;

// String SEO URL
var seo = "C# Programlama!".ToSeoFriendly(); // "c-programlama"

// TCKN Validation
bool isValid = 12345678901L.IsTRIdentityNumber();

// DateTime Extensions
var firstDay = DateTime.Today.GetFirstDayOfMonth();
```

### Helper Classes
```csharp
using UD.Core.Helper.Validation;
using UD.Core.Helper.Cryptography;

// Şifre üretimi
var password = PasswordGenerator.Default.Generate();

// AES şifreleme
string encrypted = AESHelper.Encrypt("data", "key", "iv");
```

### Auditing
```csharp
using UD.Core.Auditing;

public class Product : FullAuditedEntity<Guid, int>
{
    public string Name { get; set; }
    // CreatedAt, CreatedBy, UpdatedAt, UpdatedBy, DeletedAt, DeletedBy otomatik
}
```

### Middleware
```csharp
// Program.cs
app.UseMiddleware<TransactionMiddleware<AppDbContext>>();
app.UseMiddleware<TokenBlacklistMiddleware>();
```

## 📧 İletişim

- **Geliştirici:** Uğur DEMİREL
- **E-posta:** [ugur.demirel@outlook.com](mailto:ugur.demirel@outlook.com)
- **GitHub:** [demirelugur/UD.Core](https://github.com/demirelugur/UD.Core)

---

- **Not:** .NET 10 hedeflenmiştir. Minimum gereksinim: .NET 10 SDK
- **Ek Bilgi:** Bu README dokümanı, yapay zeka desteğiyle hazırlanmıştır.