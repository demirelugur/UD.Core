namespace UD.Core.Helper
{
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;
    using System.Globalization;
    public sealed class GlobalConstants
    {
        public static readonly string[] filesizeunits = new string[] { "bytes", "KB", "MB", "GB", "TB", "PB", "EB", "ZB", "YB" };
        public static readonly char[] turkishcharacters = new char[] { 'Ç', 'ç', 'Ğ', 'ğ', 'İ', 'ı', 'Ö', 'ö', 'Ş', 'ş', 'Ü', 'ü' };
        /// <summary>
        /// <code>
        /// new JsonSerializerSettings{MetadataPropertyHandling=MetadataPropertyHandling.Ignore,DateParseHandling=DateParseHandling.None,NullValueHandling=NullValueHandling.Include,Converters={new IsoDateTimeConverter{DateTimeStyles=DateTimeStyles.AssumeUniversal},new StringEnumConverter()}};
        /// </code>
        /// </summary>
        public static readonly JsonSerializerSettings jsonserializersettings = new JsonSerializerSettings
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
        public sealed class _date
        {
            public const string ddMMyyyy_HHmmss = "dd.MM.yyyy HH:mm:ss";
            public const string ddMMyyyy_HHmm = "dd.MM.yyyy HH:mm";
            public const string ddMMyyyy = "dd.MM.yyyy";
            public const string full_datetime = "dddd, dd MMMM yyyy HH:mm:ss";
            public const string yyyyMMdd_HHmmss = "yyyy-MM-dd HH:mm:ss";
            public const string yyyyMMdd_HHmm = "yyyy-MM-dd HH:mm";
            public const string yyyyMMdd = "yyyy-MM-dd";
            public static readonly DateOnly sqlMinValue = new(1753, 1, 1);
            /// <summary>
            /// OLE Automation için başlangıç tarihi
            /// <para>new DateOnly(1899, 12, 30)</para>
            /// </summary>
            public static readonly DateOnly beginOfOLEAutomation = new(1899, 12, 30);
        }
        public sealed class _domain
        {
            public const string example = "https://example.com";
        }
        public sealed class _maximumlength
        {
            public const int cuzdanserino = 9;
            public const int ebyscode = 16;
            public const int eposta = 50;
            public const int hash = 64;
            public const int ipaddress = 15;
            public const int mac = 17;
            public const int ogrencino = 15;
            public const int tckn = 11;
            public const int uri = 255;
            public const int vkn = 10;
            public const int sifre = 255;
        }
        public sealed class _title
        {
            public const string ebys = "EBYS(Elektronik Belge Yönetim Sistemi)";
            public const string dys = "DYS(Doküman Yönetim Sistemi)";
            public const string isbn = "ISBN(Uluslararası Standart Kitap Numarası)";
            public const string mac = "MAC(Media Access Control)";
            public const string xss = "XSS(Cross - Site Scripting)";
        }
        public sealed class _validationerrormessage
        {
            public const string array_minlength = "{0} boş geçilemez! En az {1} adet eleman içermelidir.";
            public const string email = "{0}, geçerli bir e-Posta adresi olmalıdır!";
            public const string enumdatatype = "{0}, geçerli değildir!";
            /// <summary>
            /// 1: minimum, 2: maksimum
            /// </summary>
            public const string range = "{0}, [{1} - {2}] arasında olmalıdır!";
            public const string rangepositive = "{0}, sıfırdan büyük bir değer olmalıdır!";
            public const string required = "{0}, boş geçilemez!";
            public const string stringlength_max = "{0}, en fazla {1} karakter uzunluğunda olmalıdır.";
            /// <summary>
            /// 1: maksimum, 2: minimum
            /// </summary>
            public const string stringlength_maxmin = "{0}, en az {2}, en fazla {1} karakter uzunluğunda olmalıdır!";
            public const string stringlength_maxminequal = "{0}, tam olarak {1} karakter uzunluğunda olmalıdır!";
        }
    }
}