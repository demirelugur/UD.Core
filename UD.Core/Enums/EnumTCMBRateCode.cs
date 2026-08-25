namespace UD.Core.Enums
{
    using System.ComponentModel.DataAnnotations;
    public enum EnumTCMBRateCode : byte
    {
        /// <summary>ABD DOLARI</summary>
        [Display(Name = "ABD DOLARI")]
        USD = 1,
        /// <summary>AVUSTRALYA DOLARI</summary>
        [Display(Name = "AVUSTRALYA DOLARI")]
        AUD,
        /// <summary>DANİMARKA KRONU</summary>
        [Display(Name = "DANİMARKA KRONU")]
        DKK,
        /// <summary>AVRO</summary>
        [Display(Name = "AVRO")]
        EUR,
        /// <summary>İNGİLİZ STERLİNİ</summary>
        [Display(Name = "İNGİLİZ STERLİNİ")]
        GBP,
        /// <summary>İSVİÇRE FRANGI</summary>
        [Display(Name = "İSVİÇRE FRANGI")]
        CHF,
        /// <summary>İSVEÇ KRONU</summary>
        [Display(Name = "İSVEÇ KRONU")]
        SEK,
        /// <summary>KANADA DOLARI</summary>
        [Display(Name = "KANADA DOLARI")]
        CAD,
        /// <summary>KUVEYT DİNARI</summary>
        [Display(Name = "KUVEYT DİNARI")]
        KWD,
        /// <summary>NORVEÇ KRONU</summary>
        [Display(Name = "NORVEÇ KRONU")]
        NOK,
        /// <summary>SUUDİ ARABİSTAN RİYALİ</summary>
        [Display(Name = "SUUDİ ARABİSTAN RİYALİ")]
        SAR,
        /// <summary>JAPON YENİ</summary>
        [Display(Name = "JAPON YENİ")]
        JPY,
        /// <summary>RUMEN LEYİ</summary>
        [Display(Name = "RUMEN LEYİ")]
        RON,
        /// <summary>RUS RUBLESİ</summary>
        [Display(Name = "RUS RUBLESİ")]
        RUB,
        /// <summary>ÇİN YUANI</summary>
        [Display(Name = "ÇİN YUANI")]
        CNY,
        /// <summary>PAKİSTAN RUPİSİ</summary>
        [Display(Name = "PAKİSTAN RUPİSİ")]
        PKR,
        /// <summary>KATAR RİYALİ</summary>
        [Display(Name = "KATAR RİYALİ")]
        QAR,
        /// <summary>GÜNEY KORE WONU</summary>
        [Display(Name = "GÜNEY KORE WONU")]
        KRW,
        /// <summary>AZERBAYCAN YENİ MANATI</summary>
        [Display(Name = "AZERBAYCAN YENİ MANATI")]
        AZN,
        /// <summary>BİRLEŞİK ARAP EMİRLİKLERİ DİRHEMİ</summary>
        [Display(Name = "BİRLEŞİK ARAP EMİRLİKLERİ DİRHEMİ")]
        AED,
        /// <summary>KAZAKİSTAN TENGESİ</summary>
        [Display(Name = "KAZAKİSTAN TENGESİ")]
        KZT
    }
}