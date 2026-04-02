namespace UD.Core.Attributes
{
    using System;
    /// <summary>XSS saldırılarına karşı koruma sağlamak amacıyla, HTML içeriğini temizlemek için kullanılan bir özniteliktir. Bu öznitelik, uygulandığı property, field veya parameter üzerinde HTML sanitizasyonunu devre dışı bırakır. Bu, belirli durumlarda geliştiricilerin manuel olarak HTML içeriğini temizlemek istemesi veya belirli içeriklerin sanitizasyon gerektirmemesi durumlarında faydalı olabilir. Ancak, bu özniteliği kullanırken dikkatli olunmalıdır, çünkü HTML içeriğinin temizlenmemesi potansiyel güvenlik risklerine yol açabilir.</summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public sealed class SkipSanitizeAttribute : Attribute { }
}