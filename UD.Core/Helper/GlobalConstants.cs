namespace UD.Core.Helper
{
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;
    using System.Globalization;
    public sealed class GlobalConstants
    {
        public const string Example = "https://example.com";
        /// <summary><code>new(){MetadataPropertyHandling=MetadataPropertyHandling.Ignore,DateParseHandling=DateParseHandling.None,NullValueHandling=NullValueHandling.Include,Converters={new IsoDateTimeConverter{DateTimeStyles=DateTimeStyles.AssumeUniversal},new StringEnumConverter()}};</code></summary>
        public static readonly JsonSerializerSettings JsonSerializerSettings = new()
        {
            MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
            DateParseHandling = DateParseHandling.None,
            NullValueHandling = NullValueHandling.Include,
            Converters =
            {
                new IsoDateTimeConverter { DateTimeStyles = DateTimeStyles.AssumeUniversal },
                new StringEnumConverter()
            }
        };
        public sealed class ArrayConstants
        {
            public static readonly string[] FileSizeUnits = ["bytes", "KB", "MB", "GB", "TB", "PB", "EB", "ZB", "YB"];
            public static readonly char[] TurkishSpecialCharacters = ['Ç', 'ç', 'Ğ', 'ğ', 'İ', 'ı', 'Ö', 'ö', 'Ş', 'ş', 'Ü', 'ü'];
        }
        public sealed class DateConstants
        {
            public const string ddMMyyyy_HHmmss = "dd.MM.yyyy HH:mm:ss";
            public const string ddMMyyyy_HHmm = "dd.MM.yyyy HH:mm";
            public const string ddMMyyyy = "dd.MM.yyyy";
            public const string yyyyMMdd_HHmmss = "yyyy-MM-dd HH:mm:ss";
            public const string yyyyMMdd_HHmm = "yyyy-MM-dd HH:mm";
            public const string yyyyMMdd = "yyyy-MM-dd";
            /// <summary>SQL Server&#39;ın datetime veri türü için geçerli olan minimum tarih değeri. Bu tarih, 1 Ocak 1753&#39;ü (saat 00:00:00) temsil eder. SQL Server, datetime veri türünde 1753-01-01 tarihinden önceki değerleri kabul etmez, bu nedenle bu tarih genellikle SQL Server ile uyumlu tarih işlemlerinde referans olarak kullanılır.</summary>
            public static readonly DateOnly SqlServerMinValue = new(1753, 1, 1);
            /// <summary>OLE Automation için başlangıç tarihi <para>new DateOnly(1899, 12, 30)</para></summary>
            public static readonly DateOnly BeginOfOLEAutomation = new(1899, 12, 30);
        }
        public sealed class MaximumLengthConstants
        {
            public const int CuzdanSeriNo = 9;
            public const int EMail = 50;
            public const int Hash = 64;
            public const int IPAddress = 15;
            public const int Mac = 17;
            public const int Tckn = 11;
            public const int UserName = 20;
            public const int Vkn = 10;
        }
        public sealed class TitleConstants
        {
            public const string Isbn = "ISBN(Uluslararası Standart Kitap Numarası)";
            public const string Mac = "MAC(Media Access Control)";
            public const string Xss = "XSS(Cross - Site Scripting)";
        }
        public sealed class ValidationErrorMessageConstants
        {
            public const string ArrayMinLength = "{0} boş geçilemez! En az {1} adet eleman içermelidir.";
            public const string ArrayMaxLength = "{0} boş geçilemez! En çok {1} adet eleman içermelidir.";
            public const string EMail = "{0}, geçerli bir e-Posta adresi olmalıdır!";
            /// <summary>1: minimum, 2: maksimum</summary>
            public const string Range = "{0}, [{1} - {2}] arasında olmalıdır!";
            public const string GreaterThenZero = "{0}, sıfırdan büyük bir değer olmalıdır!";
            public const string Required = "{0}, boş geçilemez!";
            public const string StringLengthMax = "{0}, en fazla {1} karakter uzunluğunda olmalıdır.";
            /// <summary>1: maksimum, 2: minimum</summary>
            public const string StringLengthBetweenMaxMin = "{0}, en az {2}, en fazla {1} karakter uzunluğunda olmalıdır!";
            public const string StringLengthEqualMaxMin = "{0}, tam olarak {1} karakter uzunluğunda olmalıdır!";
        }
    }
}