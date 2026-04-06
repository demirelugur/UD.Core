namespace UD.Core.Attributes
{
    using System;
    using UD.Core.Helper.Database;
    /// <summary><see cref="ChangeEntry"/> üzerinde yapılan değişikliklerin audit log&#39;lara kaydedilmemesi için kullanılan bir özniteliktir. Bu öznitelik, uygulandığı sınıf veya property üzerinde yapılan değişikliklerin audit log&#39;lara dahil edilmemesini sağlar. Bu, belirli durumlarda geliştiricilerin bazı değişikliklerin audit log&#39;lara kaydedilmesini istememesi veya belirli içeriklerin audit log&#39;lara dahil edilmemesi durumlarında faydalı olabilir. Ancak, bu özniteliği kullanırken dikkatli olunmalıdır, çünkü audit log&#39;ların eksik olması potansiyel güvenlik risklerine yol açabilir.</summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public sealed class SkipAuditLogAttribute : Attribute { }
}