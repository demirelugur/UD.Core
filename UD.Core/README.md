# UD.Core

Kurumsal projelerde tekrar kullanılabilir, doğrulama attribute'ları, yardımcı sınıflar, extension method'lar ve servis entegrasyonları içeren bir .NET Core kütüphanesidir. Local NuGet paketi olarak dağıtılır.

## Özellikler

- **Gelişmiş Doğrulama Attribute'ları:** TCKN, Kurum Sicil No, e-Posta, ISBN, IP, MAC, Telefon, Tarih, URL, Pozitif Sayı vb. için hazır validasyonlar.
- **Yardımcı Sınıflar:** Sahte Veri Üretimi, e-Posta Gönderimi, SMTP Ayarları, Şifreleme, Dosya İşlemleri, Dapper, Parola Üretimi vb.
- **Extension Method'lar:** Koleksiyon, String, DateTime, IQueryable, Object, PropertyInfo, Uri vb. gibi temel tipler için pratik uzantılar.
- **Enum ve Sonuç Tipleri:** Standart hata yönetimi ve veri modellemesi için Enum ve Result tipleri.

## Kullanım

### 1. Doğrulama Attribute'ları

```csharp
public class PersonelModel
{
    [Validation_Tckn]
    [DefaultValue(null)]
    public long? TCKN { get; set; }

    [Validation_Required]
    [EnumDataType(typeof(MonthTR), ErrorMessage = _validationerrormessage.enumdatatype)]
    [DefaultValue(MonthTR.oca)]
    public MonthTR ay { get; set; }

    [Validation_MinDate]
    [DefaultValue(null)]
    public DateOnly? tarih { get; set; }

    [Validation_Required]
    [Validation_Includes(true, "tumu", "basarili", "basarisiz")]
    [DefaultValue("tumu")]
    public string durum { get; set; }

    [Validation_StringLength(_maximumlength.uri)]
    [Validation_UrlHttp]
    [DefaultValue("")]
    public string? src_tr { get; set; }

    [DefaultValue_GuidEmpty]
    public Guid uid { get; set;}
}
```

### 2. Yardımcı Sınıflar

```csharp
var password = PasswordGenerator.Default.Generate();
var isvalid = StrongPasswordValid.TryIsWarning(password, "Ad", "SOYAD", "tr", out string[] _warnings);
*****************
var tcknvalid = OrtakTools._is.IsTCKimlikNo(12345678901);
*****************
var sample = new {
    key = 1
};
var valid = OrtakTools._try.TryGetProperty(sample, "key", out int _outvalue);
*****************
public record class sampleogrenciinfo(long tckn, string ogrencino, string ad, string soyad);
var data = new BogusGenericFakeDataGenerator().GenerateArray<sampleogrenciinfo>(4);
```

### 3. Extension Method'lar

```csharp
var list = new List<int> { 1, 2, 3 };
var hasitems = !list.IsNullOrCountZero();
*****************
var date = DateTime.Today;
var lastdayofmonth = date.GetLastDayOfMonth();
*****************
IFormCollection form = ...
var id = form.ParseOrDefault<int>("id");
var nullableid = form.ParseOrDefault<int?>("nullableid");
var eposta = form.ParseOrDefault<MailAddress>("eposta");
var src = form.ParseOrDefault<Uri>("src");
```

## Geri Bildirim

- Hatalar ve öneriler için: 📧 [ugur.demirel@outlook.com](mailto:ugur.demirel@outlook.com)